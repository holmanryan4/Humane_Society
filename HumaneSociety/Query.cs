﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch(InvalidOperationException e)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            
            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }


        //// TODO Items: ////
        
        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            
            switch (crudOperation)
            {
                case "create":
                    //add employee to the database
                    db.Employees.InsertOnSubmit(employee);
                    db.SubmitChanges();
                    break;
                case "read":
                    db.Employees.GetOriginalEntityState(employee);
                    db.SubmitChanges();
                    break;
                case "update":
                    db.Employees.InsertOnSubmit(employee);
                    db.SubmitChanges();
                    break;
                case "delete":
                    db.Employees.DeleteOnSubmit(employee);
                    db.SubmitChanges();
                    break;
            }
        }

        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            if (animal == null)

            {
                throw new ArgumentException("animal");
            }
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();

        }

        internal static Animal GetAnimalByID(int id)
        {
            return db.Animals.Where(a => a.AnimalId == id).FirstOrDefault();
        }

        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {
            var animalInDatabase = GetAnimalByID(animalId);
            foreach (KeyValuePair<int, string> animal in updates)
            {
                switch (animal.Key)
                {

                    case 1:

                        animalId = animalInDatabase.AnimalId;
                        db.Categories.Where(c => c.CategoryId == animalId).FirstOrDefault();
                        db.SubmitChanges();
                        return;
                    case 2:
                        animalId = animalInDatabase.AnimalId;
                        db.Animals.Where(a => a.Name == animal.Value).FirstOrDefault();
                        db.SubmitChanges();
                        return;
                       
                    case 3:
                        animalId = animalInDatabase.AnimalId;
                        db.Animals.Where(a => a.Age == int.Parse(animal.Value)).FirstOrDefault();
                        db.SubmitChanges();   
                        return;
                        
                    case 4:
                        animalId = animalInDatabase.AnimalId;
                        db.Animals.Where(a => a.Demeanor == animal.Value).FirstOrDefault();
                        db.SubmitChanges();
                        return;
                       
                    case 5:
                        animalId = animalInDatabase.AnimalId;
                        db.Animals.Where(a => a.KidFriendly == bool.Parse(animal.Value)).FirstOrDefault();
                        db.SubmitChanges();
                        return;
                    case 6:
                        animalId = animalInDatabase.AnimalId;
                        db.Animals.Where(a => a.PetFriendly == bool.Parse(animal.Value)).FirstOrDefault();
                        db.SubmitChanges();
                        return;
                    case 7:
                        animalId = animalInDatabase.AnimalId;
                        db.Animals.Where(a => a.Weight == int.Parse(animal.Value)).FirstOrDefault();
                        db.SubmitChanges();
                        return;
                    case 8:
                        Console.WriteLine("Thanks for updating the system!");
                        break;
                        
                    default:
                        Console.WriteLine("Thats not a valid option please choose again!");

                        return;
                        
                }
                
            }
           


        }

        internal static void RemoveAnimal(Animal animal)
        {
            throw new NotImplementedException();
        }
        
        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
        {
            //Animal animal = new Animal();
            //animals = IQueryable < Animal >
            IQueryable<Animal> animals = db.Animals;

            //foreach (KeyValuePair<int, Person> employee in employees) EXAMPLE
            foreach (KeyValuePair<int, string> newAnimal in updates)
            {
                switch (newAnimal.Key)
                {

                    case 1:
                        animals = animals.Where(animal => animal.CategoryId == GetCategoryId(newAnimal.Value));
                        return animals;
                        break;
                    case 2:
                        animals = animals.Where(animal => animal.Name == newAnimal.Value);
                        return animals;
                        break;
                    case 3:
                        animals = animals.Where(animal => animal.Age == int.Parse(newAnimal.Value));
                        return animals;
                        break;
                    case 4:
                        animals = animals.Where(animal => animal.Demeanor == newAnimal.Value);
                        return animals;
                        break;
                    case 5:
                        animals = animals.Where(animal => animal.KidFriendly == bool.Parse(newAnimal.Value));
                        return animals;
                        break;
                    case 6:
                        animals = animals.Where(animal => animal.PetFriendly == bool.Parse(newAnimal.Value));
                        return animals;
                        break;
                    case 7:
                        animals = animals.Where(animal => animal.Weight == int.Parse(newAnimal.Value));
                        return animals;
                        break;
                    case 8:
                        animals = animals.Where(animal => animal.AnimalId == int.Parse(newAnimal.Value));
                        return animals;
                        break;
                    default:
                        return animals;
                        break;
                }
                
            }
            return animals;
        }
         
        // TODO: Misc Animal Thing
        internal static int GetCategoryId(string categoryName)
        {
            return db.Categories.Where(c => c.Name == categoryName).Select(s => s.CategoryId).FirstOrDefault();
        }
        
        internal static Room GetRoom(int animalId)
        {
            return db.Rooms.Where(r => r.RoomNumber == animalId).FirstOrDefault();
        }
        
        internal static int GetDietPlanId(string dietPlanName)
        {
            return db.DietPlans.Where(d => d.Name == dietPlanName).Select(s => s.DietPlanId).FirstOrDefault();
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            Adoption newAdoption = new Adoption();
            newAdoption.AnimalId = animal.AnimalId;
            newAdoption.ClientId = client.ClientId;
            newAdoption.ApprovalStatus = "pending";
            newAdoption.AdoptionFee = 50;
            newAdoption.PaymentCollected = false;
            db.Adoptions.InsertOnSubmit(newAdoption);
            db.SubmitChanges();            
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            return db.Adoptions.Where(a => a.ApprovalStatus == "pending");
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {

            var adoption1 = db.Adoptions.Where(a => a.AnimalId == adoption.AnimalId).Single();
            adoption1.ApprovalStatus = isAdopted.ToString();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
          
            var shots1 = db.AnimalShots.Where(shot => shot.AnimalId == animal.AnimalId);
            return shots1;

        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            throw new NotImplementedException();
        }
    }
}