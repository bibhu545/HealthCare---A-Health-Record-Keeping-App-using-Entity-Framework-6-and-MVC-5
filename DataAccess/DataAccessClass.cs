using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using DataModels;

namespace DataAccess
{
    public class DataAccessClass
    {
        String connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        SqlConnection conn = null;
        public int SaveUserToDB(User model)
        {
            using (var context = new ConnectionString())
            {
                health_users user = new health_users()
                {
                    firstname = model.FirstName,
                    lastname = model.LastName,
                    email = model.Email,
                    password = model.Password,
                    activationlink = model.SecurityCode
                };
                context.health_users.Add(user);
                context.SaveChanges();
                return user.userid;
            }
        }
        public User GetCodeFromDB(User model, int id)
        {
            using (var context = new ConnectionString())
            {
                health_users user = context.health_users.Where(x => x.userid == id).FirstOrDefault();
                if(user != null)
                {
                    model.SecurityCode = user.activationlink;
                    model.status = 1;
                }
                else
                {
                    model.status = -1;
                }
                return model;
            }
        }
        public int ActivaeUserToDB(User model)
        {
            using (var context = new ConnectionString())
            {
                health_users user = context.health_users.Where(x => x.email == model.Email).FirstOrDefault();
                if (user != null)
                {
                    user.active = 1;
                    context.SaveChanges();
                    model.status = 1;
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
        public User LoginFromDB(String email, String password)
        {
            using (var context = new ConnectionString())
            {
                health_users user = context.health_users.Where(x => x.email.Equals(email) && x.password.Equals(password)).FirstOrDefault();
                if (user != null)
                {
                    User loggedUser = new User()
                    {
                        status = 1,
                        UserId = user.userid,
                        FirstName = user.firstname,
                        LastName = user.lastname,
                        Email = user.email,
                        Password = user.password,
                        Address = user.address,
                        Phone = user.phone,
                        Profile = user.profile,
                        SecurityCode = user.activationlink,
                        IsActive = user.active == 1 ? true : false
                    };
                    return loggedUser;
                }
                else
                {
                    return new User() { status = -1 };
                }
            }
        }
        public User UpdateUserToDB(User model)
        {
            using (var context = new ConnectionString())
            {
                health_users user = context.health_users.FirstOrDefault(x => x.userid == model.UserId);
                if(user != null)
                {
                    user.firstname = model.FirstName;
                    user.lastname = model.LastName;
                    user.email = model.Email;
                    user.phone = model.Phone;
                    user.address = model.Address;
                    user.profile = model.Profile;
                    context.SaveChanges();
                    model.status = 1;
                }
                else
                {
                    model.status = -1;
                }
                return model;
            }
        }
        public User UpdatePasswordToDB(User model)
        {
            using (var context = new ConnectionString())
            {
                health_users user = context.health_users.Where(x => x.userid == model.UserId).FirstOrDefault();
                if (user != null)
                {
                    user.password = model.Password;
                    context.SaveChanges();
                    model.status = 1;
                    return model;
                }
                else
                {
                    return new User() { status = -1 };
                }
            }
        }
        public int AddHospitalToDB(Hospital model)
        {
            using (var context = new ConnectionString())
            {
                hospitals hospital = new hospitals()
                {
                    userid = model.UserId,
                    hospitalname = model.HospitalName,
                    address = model.Address,
                    phone1 = model.Phone1,
                    phone2 = model.Phone2,
                    email = model.Email,
                    isprimary = model.IsPrimary
                };
                context.hospitals.Add(hospital);
                context.SaveChanges();
                return hospital.hospitalid;
            }
        }
        public List<Hospital> GetHospitalsFromDB(int userid)
        {
            using (var context = new ConnectionString())
            {
                List<Hospital> hospitals = context.hospitals.Where(x => x.userid == userid).Select(x => new Hospital()
                {
                    HospitalId = x.hospitalid,
                    HospitalName = x.hospitalname,
                    Address = x.address,
                    Phone1 = x.phone1,
                    Phone2 = x.phone2,
                    Email = x.email,
                    IsPrimary = x.isprimary
                }).ToList();
                var orderedHospitals = from h in hospitals orderby h.IsPrimary descending select h;
                return orderedHospitals.ToList();
            }
        }
        public List<Speciality> GetSpecialitiesFromDB()
        {
            using (var context = new ConnectionString())
            {
                return context.speciality.Select(x => new Speciality()
                {
                    SpecialityId = x.specialityid,
                    SpecialityName = x.speciality1
                }).ToList();
            }
        }
        public int DeleteHospitalFromDB(int hospitalid)
        {
            using (var context = new ConnectionString())
            {
                return context.DeleteHospital(hospitalid);
            }
        }
        public Hospital GetHospitalDetailsFromDB(int userid, int id)
        {
            using (var context = new ConnectionString())
            {
                hospitals hospital = context.hospitals.FirstOrDefault(x => x.hospitalid == id && x.userid == userid);
                Hospital model = new Hospital();
                if(hospital != null)
                {
                    model.status = 1;
                    model.HospitalId = hospital.hospitalid;
                    model.UserId = hospital.userid;
                    model.HospitalName = hospital.hospitalname;
                    model.Address = hospital.address;
                    model.Phone1 = hospital.phone1;
                    model.Phone2 = hospital.phone2;
                    model.Email = hospital.email;
                    model.IsPrimary = hospital.isprimary;
                }
                else
                {
                    model.status = -1;
                }
                return model;
            }
        }
        public Hospital UpdateHospitalToDB(Hospital hospital)
        {
            using (var context = new ConnectionString())
            {
                hospital.status = -1;
                hospital.status = context.UpdateHospital(hospital.HospitalId, hospital.UserId, hospital.HospitalName, hospital.Address, hospital.Phone1, hospital.Phone2, hospital.Email, hospital.IsPrimary);
                return hospital;
            }
        }
        public Doctor AddDoctorToDB(Doctor doctor)
        {
            using (var context = new ConnectionString())
            {
                doctor.status = -1;
                doctor.status = context.CreateDoctors(doctor.UserId, doctor.HospitalId, doctor.FirstName, doctor.LastName, doctor.Speciality, doctor.Address, doctor.Phone1, doctor.Phone2, doctor.Email, doctor.IsPrimary);
                return doctor;
            }
        }
        public int DeleteDoctorFromDB(int id)
        {
            using (var context = new ConnectionString())
            {
                doctors doctor = new doctors() { doctorid = id };
                context.Entry(doctor).State = System.Data.Entity.EntityState.Deleted;
                return context.SaveChanges();
            }
        }
        public List<Doctor> GetDoctorsFromDB(int userid, int doctorid)
        {
            using (var context = new ConnectionString())
            {
                if(doctorid == 0)
                {
                    return context.GetAllDoctors(userid).Select(x => new Doctor()
                    {
                        status = 1,
                        DoctorId = x.doctorid,
                        FirstName = x.firstname,
                        HospitalName = x.hospitalname,
                        Address = x.address,
                        Phone1 = x.phone1,
                        IsPrimary = (int)x.isprimary
                    }).ToList();
                }
                else
                {
                    return context.GetDoctorDetailsV2(doctorid, userid).Select(x => new Doctor()
                    {
                        status = 1,
                        DoctorId = x.doctorid,
                        FirstName = x.firstname,
                        LastName = x.lastname,
                        HospitalId = x.hospitalid,
                        HospitalName = x.hospitalname,
                        Speciality = x.specialityid,
                        SpecialistIn = x.speciality,
                        Address = x.address,
                        Phone1 = x.phone1,
                        Phone2 = x.phone2,
                        Email = x.email,
                        IsPrimary = (int)x.isprimary
                    }).ToList();
                }
            }
        }
        public Doctor UpdateDoctorToDB(Doctor doctor)
        {
            using (var context = new ConnectionString())
            {
                doctor.status = -1;
                doctor.status = context.UpdateDoctor(doctor.DoctorId, doctor.UserId, doctor.HospitalId, doctor.FirstName, doctor.LastName, doctor.Speciality, doctor.Address, doctor.Phone1, doctor.Phone2, doctor.Email, doctor.IsPrimary);
                return doctor;
            }  
        }
        public List<Doctor> GetDoctorsByHospitalFromDB(int hospitalId)
        {
            using (var context = new ConnectionString())
            {
                return context.doctors.Where(x => x.hospitalid == hospitalId).Select(x => new Doctor() {
                    DoctorId = x.doctorid,
                    FirstName = x.firstname
                }).ToList();
            }
        }
        public List<RecordType> GetRecordTypesFromDB()
        {
            using (var context = new ConnectionString())
            {
                return context.recordtypes.Select(x => new RecordType() {
                    RecordId = x.recordid,
                    RecordTypeName = x.recordtype
                }).ToList();
            }
        }
        public Document SaveDocumentToDB(Document document, List<String> allFiles)
        {
            using (var context = new ConnectionString())
            {
                document.status = -1;
                List<documents> docs = new List<documents>();
                foreach (String path in allFiles)
                {
                    docs.Add(new documents() {
                        userid = document.UserId,
                        hospitalid = document.HospitalId,
                        doctorid = document.DoctorId,
                        issuedate = DateTime.Parse(document.IssueDate),
                        recordtype = document.DocumentType,
                        filepath = path
                    });
                }
                context.documents.AddRange(docs);
                document.status = context.SaveChanges();
                return document;
            }
        }
        public List<Document> GetFilesFromDB(int userid)
        {
            using (var context = new ConnectionString())
            {
                return context.GetFilesFromDB(userid).Select(x => new Document() {
                    DocumentId = x.documentid,
                    Path = x.filepath,
                    HospitalName = x.hospitalname,
                    DoctorName = x.firstname,
                    IssueDate = x.issuedate.ToString(),
                    DocumentType = x.recordid,
                    DocumentTypeName = x.recordtype
                }).ToList();
            }
        }
        public List<Document> GetFilteredFilesFromDBV2(int userid, DateTime startDate, DateTime endDate)
        {
            using (var context = new ConnectionString())
            {
                return context.GetFormattedFilesFromDBV2(userid, startDate, endDate).Select(x => new Document()
                {
                    DocumentId = x.documentid,
                    Path = x.filepath,
                    HospitalName = x.hospitalname,
                    DoctorName = x.firstname,
                    IssueDate = x.issuedate.ToString(),
                    DocumentType = x.recordid,
                    DocumentTypeName = x.recordtype
                }).ToList();
            }
        }
        public int DeleteDocumentFromDB(int documentid)
        {
            using (var context = new ConnectionString())
            {
                return context.DeleteDocument(documentid);
            }
        }

        public int GetTotalDoctorsFormDb(int userid)
        {
            using (var context = new ConnectionString())
            {
                return context.doctors.Where(x => x.userid == userid).Count();
            }
        }
        public int GetTotalHospitalsFormDb(int userid)
        {
            using (var context = new ConnectionString())
            {
                return context.hospitals.Where(x => x.userid == userid).Count();
            }
        }
        public int GetTotalDocumentsFormDb(int userid)
        {
            using (var context = new ConnectionString())
            {
                return context.documents.Where(x => x.userid == userid).Count();
            }
        }
    }
}
