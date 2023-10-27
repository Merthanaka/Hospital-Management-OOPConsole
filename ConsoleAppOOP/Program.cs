using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static HospitalManagementSystem.Program;

namespace HospitalManagementSystem
{
    internal class Program
    {
        public static List<Doctor> doctors = new List<Doctor>();
        public static List<Nurse> nurses = new List<Nurse>();
        public static string userN1 = "";

        [Serializable]
        public class Employee
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string PESEL { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
            public static void DisplayEmployeeList(List<Employee> employees)
            {
                Console.WriteLine("Employee List:");
                foreach (Employee e in employees)
                {
                    if (e is Doctor d)
                    {
                        Console.WriteLine($"{d.Name} {d.Surname}  ({"Doctor"} {d.Specialty})");
                    }
                    else if (e is Nurse n)
                    {
                        Console.WriteLine($"{n.Name} {n.Surname} (Nurse)");
                    }
                }
            }

            public static void DisplayALlUsersList(List<Employee> employees)
            {
                Console.WriteLine("Employee List:");
                foreach (Employee e in employees)
                {
                    if (e is Doctor d)
                    {
                        Console.WriteLine($"{d.Name} {d.Surname} ({"Doctor"} {d.Specialty})");
                    }
                    else if (e is Nurse n)
                    {
                        Console.WriteLine($"{n.Name} {n.Surname} (Nurse)");
                    }
                    else if (e is Administrator a)
                    {
                        Console.WriteLine($"{a.Name} {a.Surname} (Administrator)");
                    }
                }
            }
        }
        [Serializable]
        public class Doctor : Employee
        {
            public string Specialty { get; set; }
            public string PWZ { get; set; }
            public List<DateTime> OnCallDuties = new List<DateTime>();

            public void AddOnCallDuty(DateTime duty, List<Doctor> doctors)
            {
                DateTime t = new DateTime();
                if (OnCallDuties != null)
                    if (OnCallDuties.Count != 0 && OnCallDuties.Count >= 10)
                    {
                        throw new Exception("This doctor already has the maximum number of on-call duties for the month.");
                    }

                /*foreach (Doctor d in doctors)
                {
                    if (d.Specialty == Specialty && d.OnCallDuties.Contains(duty))
                    {
                        throw new Exception("Another doctor of this specialty already has an on-call duty on this day.");
                    }
                }*/
                try
                {
                    OnCallDuties.Add(t);
                    Console.Write("Duty assigned to the Doctor");
                }
                catch
                {
                    Console.Write("Successfully Assigned");
                }


                Console.Read();
            }
        }
        [Serializable]
        public class Nurse : Employee
        {
            public List<DateTime> OnCallDuties { get; set; }
            public void AddOnCallDuty(DateTime duty, List<Employee> doctors)
            {
                if (OnCallDuties.Count >= 10)
                {
                    throw new Exception("This Nurse already has the maximum number of on-call duties for the month.");
                }
                OnCallDuties.Add(duty);
            }
        }
        [Serializable]
        public class Administrator : Employee
        {

            public static void EditEmployee(List<Employee> employees, Employee employee, string name, string surname, string pesel, string username, string password)
            {
                employee.Name = name;
                employee.Surname = surname;
                employee.PESEL = pesel;
                employee.Username = username;
                employee.Password = password;
            }

            public static void AddEmployee(List<Employee> employees, string type, string name, string surname, string pesel, string username, string password, string speciality)
            {
                Employee e = null;
                if (type == "Doctor")
                {
                    e = new Doctor { Name = name, Surname = surname, PESEL = pesel, Username = username, Password = password, Role = "Doctor", Specialty = speciality };
                }
                else if (type == "Nurse")
                {
                    e = new Nurse { Name = name, Surname = surname, PESEL = pesel, Username = username, Password = password, Role = "Nurse" };
                }
                else if (type == "Administrator")
                {
                    e = new Administrator { Name = name, Surname = surname, PESEL = pesel, Username = username, Password = password, Role = "Administrator" };
                }
                if (e != null)
                {
                    employees.Add(e);
                }
            }
        }


        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                using (Stream stream = File.Open("employees.bin", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    if (stream != null)
                        employees = (List<Employee>)bin.Deserialize(stream);
                }
            }
            catch
            {
                Console.Write("File is empty");
            }


            Doctor d1 = new Doctor { Name = "John", Surname = "Doe", PESEL = "12345678901", Username = "johndoe", Password = "password", Role = "Doctor", Specialty = "Surgeon" };
            Doctor d2 = new Doctor { Name = "Jane", Surname = "Doe", PESEL = "98765432109", Username = "janedoe", Password = "password", Role = "Doctor", Specialty = "Cardiologist" };

            Nurse n1 = new Nurse { Name = "James", Surname = "Smith", PESEL = "23456789012", Username = "jamessmith", Password = "password", Role = "Nurse" };
            Nurse n2 = new Nurse { Name = "Emily", Surname = "Smith", PESEL = "87654321098", Username = "emilysmith", Password = "password", Role = "Nurse" };

            Administrator a1 = new Administrator { Name = "Mustafa", Surname = "Riaz", PESEL = "34567890123", Username = "mustafa", Password = "password", Role = "Administrator" };
            Administrator a2 = new Administrator { Name = "Sarah", Surname = "Williams", PESEL = "76543210987", Username = "sarahwilliams", Password = "password", Role = "Administrator" };

            doctors.Add(d1);
            doctors.Add(d2);
            nurses.Add(n1);
            nurses.Add(n2);
            
            /*  employees.Add(d1);
              employees.Add(d2);
              employees.Add(n1);
              employees.Add(n2);
              employees.Add(a1);
              employees.Add(a2);*//*
              d1.AddOnCallDuty(new DateTime(2023, 1, 1), doctors);
              d1.AddOnCallDuty(new DateTime(2023, 1, 2), doctors);
              d1.AddOnCallDuty(new DateTime(2023, 1, 3), doctors);*/
            loginMenu(employees);

            using (Stream stream = File.Open("employees.bin", FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, employees);
            }

        }
        static void loginMenu(List<Employee> employees)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the Hospital Administration System");
                Console.WriteLine("\nEnter your user name: ");
                string username = Console.ReadLine();
                Console.WriteLine("\nEnter your password: ");
                string userPassword = Console.ReadLine();
                userN1 = username;
                int count = 0;
                for (int i = 0; i < employees.Count; i++)
                {

                    if (username == employees[i].Username && userPassword == employees[i].Password)
                    {
                        if (employees[i].Role == "Doctor" || employees[i].Role == "Nurse")
                        {

                            Console.Clear();
                            Console.WriteLine("\n\n\n\t\tHospital Administration System\n\n");
                            doctorMenu(employees);

                        }
                        else if (employees[i].Role == "Administrator")
                        {

                            Console.Clear();
                            Console.WriteLine("\n\n\n\t\tHospital Administration System\n\n");
                            adminMenu(employees);

                        }
                        else
                        {
                            Console.Write("Wrong User");
                        }
                        Console.Read();
                    }
                }
                Console.Write("Login Failed");
                Console.Read();
            }
        }
        static void doctorMenu(List<Employee> employees)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1-Show the list of all doctors");
                Console.WriteLine("2-Show Assigned Duties");
                Console.WriteLine("3-Exit");
                string opt = "";
                opt = Console.ReadLine();
                if (opt == "1")
                {
                    Employee.DisplayEmployeeList(employees);
                    Console.ReadKey();
                }
                else if (opt == "2")
                {
                    Doctor d1 = new Doctor();
                    foreach (Doctor d in doctors)
                    {
                        if (userN1 == d.Username)
                        {
                            d1 = d;
                        }
                    }
                    foreach (DateTime t in d1.OnCallDuties)
                    {
                        Console.WriteLine(t);
                    }
                    Console.Read();
                }
                else if (opt == "3")
                {
                    using (Stream stream = File.Open("employees.bin", FileMode.Create))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, employees);
                    }
                    break;
                }
            }
        }
        static void adminMenu(List<Employee> employees)
        {
            while (true)
            {
                Console.WriteLine("1-Show the list of all users");
                Console.WriteLine("2-Add new user");
                Console.WriteLine("3-Edit user");
                Console.WriteLine("4-Assign Duty");
                Console.WriteLine("5-Exit");
                string opt = "";
                opt = Console.ReadLine();
                if (opt == "1")
                {
                    Employee.DisplayALlUsersList(employees);
                    Console.ReadKey();
                }
                else if (opt == "2")
                {
                    Console.WriteLine("Enter the user Name");
                    String name = Console.ReadLine();
                    Console.WriteLine("Enter the user Surname");
                    String surName = Console.ReadLine();
                    Console.WriteLine("Enter the user userName");
                    String username = Console.ReadLine();
                    Console.WriteLine("Enter the user Password");
                    String password = Console.ReadLine();
                    Console.WriteLine("Enter the user Pesel Number");
                    String pasel = Console.ReadLine();
                    Console.WriteLine("Enter the user Role");
                    String role = Console.ReadLine();
                    string speciality = "null";
                    if (role == "Doctor")
                    {
                        Console.WriteLine("Enter the doctor Speciality");
                        speciality = Console.ReadLine();
                    }
                    Administrator.AddEmployee(employees, role, name, surName, pasel, username, password, speciality);
                }
                else if (opt == "3")
                {
                    Employee m = new Employee();
                    m = null;
                    Console.WriteLine("Enter the username emoloyee to edit");
                    string finduser = Console.ReadLine();
                    foreach (Employee e in employees)
                    {
                        if (e.Username == finduser)
                        {
                            m = e;
                        }
                    }
                    if (m != null)
                    {

                        Console.WriteLine("Enter the user Name");
                        String name = Console.ReadLine();
                        Console.WriteLine("Enter the user Surname");
                        String surName = Console.ReadLine();
                        Console.WriteLine("Enter the user userName");
                        String username = Console.ReadLine();
                        Console.WriteLine("Enter the user Password");
                        String password = Console.ReadLine();
                        Console.WriteLine("Enter the user Pesel Number");
                        String pasel = Console.ReadLine();
                        Administrator.EditEmployee(employees, m, name, surName, pasel, username, password);
                    }
                    else
                    {
                        Console.WriteLine("No emplyee with this username exists in the system");
                    }

                }
                else if (opt == "4")
                {
                    DateTime t = new DateTime();
                    Doctor m = new Doctor();
                    //dm.AddOnCallDuty(d,doctors);
                    Console.WriteLine("\nEnter the doctor name to assign duty");
                    string name = Console.ReadLine();
                    foreach (Doctor e in doctors)
                    {
                        if (e.Username == name)
                        {
                            m = e;
                            m.AddOnCallDuty(t, doctors);
                            Console.WriteLine("Hello");
                        }
                    }
                    Console.Read();
                }
                else if (opt == "5")
                {
                    using (Stream stream = File.Open("employees.bin", FileMode.Create))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, employees);
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Wrong Input");
                }
            }
        }

    }
}
