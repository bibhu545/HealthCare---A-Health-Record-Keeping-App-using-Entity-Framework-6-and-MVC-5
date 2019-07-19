using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net.Mail;
using System.Net;
using DataAccess;
using DataModels;

namespace BusinessLayer
{
    public class FileData
    {
        public List<int> DocumentId = new List<int>();
        public List<String> FileName = new List<string>();
        public List<String> Extension = new List<string>();
        public List<String> FilePath = new List<string>();
        public List<String> IssueDate = new List<string>();
        public List<String> HospitalName = new List<string>();
        public List<String> DoctorName = new List<string>();
        public List<String> RecordType = new List<string>();
    }
    public class BusinessClass
    {
        public User CreateUser(User user)
        {
            Random random = new Random();
            int otp = random.Next(1999, 9999);

            String mailHead = "Complete your registration at Healthcare.";
            String mailBody = "Please enter " + otp.ToString() + " to confirm your membership";
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("7c73e9fdbcbfbf", "4708f0b90c37e9"),
                EnableSsl = true
            };
            client.Send("bibhu@demomail.com", user.Email, mailHead, mailBody);

            user.SecurityCode = otp.ToString();

            user.Email = user.Email.Replace("'","''");
            user.FirstName = user.FirstName.Replace("'","''");
            user.LastName = user.LastName.Replace("'","''");
            user.Password = user.Password.Replace("'","''");

            user.status = new DataAccessClass().SaveUserToDB(user);
            return user;
        }
        public User GetCode(User user, int id)
        {
            return new DataAccessClass().GetCodeFromDB(user, id);
        }
        public int ActivateUser(User user)
        {
            return new DataAccessClass().ActivaeUserToDB(user);
        }
        public User Login(String email, String password)
        {
            email.Replace(",", "''");
            password.Replace(",", "''");
            return new DataAccessClass().LoginFromDB(email, password);
        }
        public User EditUserDetails(User user)
        {
            user.Email = user.Email.Replace("'", "''");
            user.FirstName = user.FirstName.Replace("'", "''");
            user.LastName = user.LastName.Replace("'", "''");
            user.Password = user.Password.Replace("'", "''");
            user.Phone = user.Phone.Replace("'", "''");
            user.Profile = user.Profile.Replace("'", "''");
            return new DataAccessClass().UpdateUserToDB(user);
        }
        public User updatePassword(User user)
        {
            user.Password = user.Password.Replace("'", "''");
            return new DataAccessClass().UpdatePasswordToDB(user);
        }

        public int AddHospital(Hospital hospital)
        {
            hospital.HospitalName = hospital.HospitalName.Replace("'", "''");
            hospital.Address = hospital.Address.Replace("'", "''");
            hospital.Phone1 = hospital.Phone1.Replace("'", "''");
            hospital.Phone2 = hospital.Phone2.Replace("'", "''");
            hospital.Email = hospital.Email.Replace("'", "''");
            return new DataAccessClass().AddHospitalToDB(hospital);
        }
        public List<Hospital> GetHospitals(int id)
        {
            return new DataAccessClass().GetHospitalsFromDB(id);
        }
        public List<Speciality> GetSpecialities()
        {
            return new DataAccessClass().GetSpecialitiesFromDB();
        }
        public int DeleteHospital(int hospitalid)
        {
            return new DataAccessClass().DeleteHospitalFromDB(hospitalid);
        }
        public Hospital GetHospitalDetails(int userid, int id)
        {
            return new DataAccessClass().GetHospitalDetailsFromDB(userid, id);
        }
        public Hospital EditHospitalDetails(Hospital hospital)
        {
            hospital.HospitalName = hospital.HospitalName.Replace("'", "''");
            hospital.Address = hospital.Address.Replace("'", "''");
            hospital.Phone1 = hospital.Phone1.Replace("'", "''");
            hospital.Phone2 = hospital.Phone2.Replace("'", "''");
            hospital.Email = hospital.Email.Replace("'", "''");
            return new DataAccessClass().UpdateHospitalToDB(hospital);
        }

        public Doctor GetDoctorDetails(int userid, int doctorid)
        {
            List<Doctor> doctors = new DataAccessClass().GetDoctorsFromDB(userid, doctorid);
            int records = doctors.Count;
            Doctor doctor = new Doctor();
            if (records > 0)
            {
                doctor.status = 1;
                doctor = doctors.FirstOrDefault();
            }
            else
            {
                doctor.status = -1;
            }
            return doctor;
        }
        public int DeleteDoctor(int doctorid)
        {
            return new DataAccessClass().DeleteDoctorFromDB(doctorid);
        }
        public List<Doctor> GetDoctors(int userid)
        {
            return new DataAccessClass().GetDoctorsFromDB(userid, 0);
        }
        public Doctor AddDoctor(Doctor doctor)
        {
            doctor.FirstName = doctor.FirstName.Replace("'", "''");
            doctor.LastName = doctor.LastName.Replace("'", "''");
            doctor.Address = doctor.Address.Replace("'", "''");
            doctor.Email = doctor.Email.Replace("'", "''");
            doctor.Phone1 = doctor.Phone1.Replace("'", "''");
            doctor.Phone2 = doctor.Phone2.Replace("'", "''");
            return new DataAccessClass().AddDoctorToDB(doctor);
        }
        public Doctor EditDoctor(Doctor doctor)
        {
            doctor.FirstName = doctor.FirstName.Replace("'", "''");
            doctor.LastName = doctor.LastName.Replace("'", "''");
            doctor.Address = doctor.Address.Replace("'", "''");
            doctor.Email = doctor.Email.Replace("'", "''");
            doctor.Phone1 = doctor.Phone1.Replace("'", "''");
            doctor.Phone2 = doctor.Phone2.Replace("'", "''");
            return new DataAccessClass().UpdateDoctorToDB(doctor);
        }

        public List<RecordType> GetRecordTypes()
        {
            return new DataAccessClass().GetRecordTypesFromDB();
        }
        public Document SaveDocument(Document document, List<String> allFiles)
        {
            return new DataAccessClass().SaveDocumentToDB(document, allFiles);
        }
        public List<Doctor> GetDoctorsByHospital(int hospitalid)
        {
            return new DataAccessClass().GetDoctorsByHospitalFromDB(hospitalid);
        }
        public FileData GetFiles(int userid)
        {
            List<Document> docs = new DataAccessClass().GetFilesFromDB(userid);
            FileData fileData = new FileData();
            foreach (Document doc in docs)
            {
                String file = doc.Path;
                String fName = file.Substring(file.LastIndexOf("/"), file.Length - file.LastIndexOf("/")).Substring(1);
                String extension = fName.Substring(fName.LastIndexOf(".")).Substring(1); ;
                fileData.FileName.Add(fName.Replace("." + extension, "").Substring(7));
                fileData.Extension.Add(extension);
                fileData.DocumentId.Add(doc.DocumentId);
                fileData.FilePath.Add(doc.Path);
                fileData.IssueDate.Add(doc.IssueDate);
                fileData.HospitalName.Add(doc.HospitalName);
                fileData.DoctorName.Add(doc.DoctorName);
                fileData.RecordType.Add(doc.DocumentTypeName);
            }
            return fileData;
        }
        public FileData GetFilteredFilesV2(int userid, DateTime startDate, DateTime endDate)
        {
            List<Document> docs = new DataAccessClass().GetFilteredFilesFromDBV2(userid, startDate, endDate);
            FileData fileData = new FileData();
            foreach (Document doc in docs)
            {
                String file = doc.Path;
                String fName = file.Substring(file.LastIndexOf("/"), file.Length - file.LastIndexOf("/")).Substring(1);
                String extension = fName.Substring(fName.LastIndexOf(".")).Substring(1); ;
                fileData.FileName.Add(fName.Replace("." + extension, "").Substring(7));
                fileData.Extension.Add(extension);
                fileData.DocumentId.Add(doc.DocumentId);
                fileData.FilePath.Add(doc.Path);
                fileData.IssueDate.Add(doc.IssueDate);
                fileData.HospitalName.Add(doc.HospitalName);
                fileData.DoctorName.Add(doc.DoctorName);
                fileData.RecordType.Add(doc.DocumentTypeName);
            }
            return fileData;
        }
        public int DeleteDocument(int documentid)
        {
            return new DataAccessClass().DeleteDocumentFromDB(documentid);
        }

        public int GetTotalDoctors(int userid)
        {
            return new DataAccessClass().GetTotalDoctorsFormDb(userid);
        }
        public int GetTotalHospitals(int userid)
        {
            return new DataAccessClass().GetTotalHospitalsFormDb(userid);
        }
        public int GetTotalDocuments(int userid)
        {
            return new DataAccessClass().GetTotalDocumentsFormDb(userid);
        }


        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public string RandomAlphaNumericString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(2, true));
            builder.Append(RandomNumber(10, 99));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }
    }
}
