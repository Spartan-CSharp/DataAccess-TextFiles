using System;
using System.Collections.Generic;
using System.IO;

using DataAccessLibrary;
using DataAccessLibrary.Models;

using Microsoft.Extensions.Configuration;

namespace TextFileUI
{
	internal static class Program
	{
		private static IConfiguration _configuration;
		private static IDataLogic _data;

		private static void Main(string[] args)
		{
			InitializeConfiguration();
			InitializeDatabaseConnection();
			ProgramIntro();
			ProgramLoop();
			ProgramOutro();
		}

		private static void InitializeConfiguration()
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");
			_configuration = builder.Build();
		}

		private static void InitializeDatabaseConnection()
		{
			_data = new DataLogic(_configuration, DBTYPES.CSVFile);
		}

		private static void ProgramIntro()
		{
			Console.WriteLine();
			Console.WriteLine("Welcome to Text File CRUD!");
			Console.WriteLine("by Pierre Plourde");
			Console.WriteLine();
		}

		private static void ProgramLoop()
		{
			char action;

			do
			{
				action = GetActionSelection();
				switch ( action )
				{
					case 'c':
						PersonModel newPerson = CreateNewPerson();
						DisplayPerson(newPerson);
						break;
					case 'r':
						PersonModel person = RetrievePerson();
						if ( person != null )
						{
							DisplayPerson(person);
						}

						break;
					case 'u':
						PersonModel personToUpdate = RetrievePerson();
						if ( personToUpdate != null )
						{
							personToUpdate.UpdatePerson();
							DisplayPerson(personToUpdate);
						}

						break;
					case 'd':
						DeletePerson();
						break;
					case 'x':
						break;
					default:
						break;
				}
			} while ( action != 'x' );
		}

		private static char GetActionSelection()
		{
			char output;

			do
			{
				Console.Write("What action do you want to perform (C = Create/R = Retrieve/U = Update/D = Delete/X = Exit): ");
				string input = Console.ReadLine().ToLower();

				output = input.Length > 0 ? input[0] : 'z';

				if ( output != 'c' && output != 'r' && output != 'u' && output != 'd' && output != 'x' )
				{
					Console.WriteLine("That is not a valid selection.  Please try again.");
					Console.WriteLine();
				}
			} while ( output != 'c' && output != 'r' && output != 'u' && output != 'd' && output != 'x' );

			return output;
		}

		private static PersonModel CreateNewPerson()
		{
			PersonModel output = new PersonModel();

			Console.WriteLine("Please enter all values; empty values are not allowed");
			do
			{
				Console.Write("First Name: ");
				output.FirstName = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.FirstName) );
			do
			{
				Console.Write("Last Name: ");
				output.LastName = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.LastName) );
			string isactive;

			Console.Write("Is Active (True/False): ");
			isactive = Console.ReadLine().ToLower();

			output.IsActive = isactive.Length != 0 && isactive[0] == 't';

			Console.WriteLine();
			bool selectionvalid;
			int selection;
			do
			{
				Console.Write("Please select: 1 = No address, 2 = Create a new address: ");
				selectionvalid = int.TryParse(Console.ReadLine(), out selection);
				if ( !selectionvalid || selection < 1 || selection > 2 )
				{
					Console.WriteLine("That is not a valid selection.  Please try again.");
				}
			} while ( !selectionvalid || selection < 1 || selection > 2 );

			if ( selection != 1 )
			{
				bool done = false;
				do
				{
					if ( selection == 2 )
					{
						AddressModel address = CreateNewAddress();
						output.Addresses.Add(address);
					}

					do
					{
						Console.Write("Please select: 1 = No more addresses, 2 = Create another new address: ");
						selectionvalid = int.TryParse(Console.ReadLine(), out selection);
						if ( !selectionvalid || selection < 1 || selection > 2 )
						{
							Console.WriteLine("That is not a valid selection.  Please try again.");
						}
					} while ( !selectionvalid || selection < 1 || selection > 2 );
					if ( selection == 1 )
					{
						done = true;
					}
				} while ( !done );
			}

			Console.WriteLine();
			do
			{
				Console.Write("Please select: 1 = No employer, 2 = Create a new employer: ");
				selectionvalid = int.TryParse(Console.ReadLine(), out selection);
				if ( !selectionvalid || selection < 1 || selection > 2 )
				{
					Console.WriteLine("That is not a valid selection.  Please try again.");
				}
			} while ( !selectionvalid || selection < 1 || selection > 2 );

			if ( selection == 1 )
			{
				output.Employer = null;
			}

			if ( selection == 2 )
			{
				EmployerModel employer = CreateNewEmployer();
				output.Employer = employer;
			}

			_data.SaveNewPerson(output);

			return output;
		}

		private static EmployerModel CreateNewEmployer()
		{
			EmployerModel output = new EmployerModel();

			Console.WriteLine("Please enter all values; empty values are not allowed");
			do
			{
				Console.Write("Company Name: ");
				output.CompanyName = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.CompanyName) );

			Console.WriteLine();
			bool selectionvalid;
			int selection;
			do
			{
				Console.Write("Please select: 1 = No address, 2 = Create a new address: ");
				selectionvalid = int.TryParse(Console.ReadLine(), out selection);
				if ( !selectionvalid || selection < 1 || selection > 2 )
				{
					Console.WriteLine("That is not a valid selection.  Please try again.");
				}
			} while ( !selectionvalid || selection < 1 || selection > 2 );

			if ( selection != 1 )
			{
				bool done = false;
				do
				{
					if ( selection == 2 )
					{
						AddressModel address = CreateNewAddress();
						output.Addresses.Add(address);
					}

					do
					{
						Console.Write("Please select: 1 = No more addresses, 2 = Create another new address: ");
						selectionvalid = int.TryParse(Console.ReadLine(), out selection);
						if ( !selectionvalid || selection < 1 || selection > 2 )
						{
							Console.WriteLine("That is not a valid selection.  Please try again.");
						}
					} while ( !selectionvalid || selection < 1 || selection > 2 );

					if ( selection == 1 )
					{
						done = true;
					}
				} while ( !done );
			}

			return output;
		}

		private static AddressModel CreateNewAddress()
		{
			AddressModel output = new AddressModel();

			Console.WriteLine("Please enter all values; empty values are not allowed");
			do
			{
				Console.Write("Street Address: ");
				output.StreetAddress = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.StreetAddress) );
			do
			{
				Console.Write("City: ");
				output.City = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.City) );
			do
			{
				Console.Write("State: ");
				output.State = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.State) );
			do
			{
				Console.Write("Zip Code: ");
				output.ZipCode = Console.ReadLine();
			} while ( string.IsNullOrWhiteSpace(output.ZipCode) );
			Console.WriteLine();

			return output;
		}

		private static PersonModel RetrievePerson()
		{
			PersonModel output = new PersonModel();

			List<PersonModel> people = _data.GetAllPeople();

			if ( people.Count > 0 )
			{
				bool selectionvalid;
				int personid;
				do
				{
					Console.WriteLine("Please select the person from the following list using the number:");
					for ( int i = 0; i < people.Count; i++ )
					{
						Console.WriteLine($"{i + 1}: {people[i].FirstName} {people[i].LastName}");
					}

					selectionvalid = int.TryParse(Console.ReadLine(), out personid);

					if ( !selectionvalid || personid == 0 || personid > people.Count )
					{
						Console.WriteLine("That is not a valid selection.");
					}
					else
					{
						output = people[personid - 1];
					}
				} while ( !selectionvalid || personid == 0 || personid > people.Count );
			}
			else
			{
				Console.WriteLine();
				Console.WriteLine("-- No people in list --");
				Console.WriteLine();
				output = null;
			}

			return output;
		}

		private static void UpdatePerson(this PersonModel person)
		{
			Console.WriteLine($"Current First Name: {person.FirstName}");
			Console.Write("Enter new First Name (press enter on blank line for no change): ");
			string input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				person.FirstName = input;
			}

			Console.WriteLine($"Current Last Name: {person.FirstName}");
			Console.Write("Enter new Last Name (press enter on blank line for no change): ");
			input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				person.LastName = input;
			}

			Console.WriteLine($"Current Is Active: {person.IsActive}");
			Console.Write("Enter new Is Active (press enter on blank line for no change): ");
			input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				person.IsActive = input.Length != 0 && input[0] == 't';
			}

			if ( person.Addresses.Count == 0 )
			{
				Console.Write("This person has no addresses.");
			}
			else
			{
				Console.WriteLine("Addresses for the Person:");
				for ( int i = 0; i < person.Addresses.Count; i++ )
				{
					Console.WriteLine($"{i + 1}: {person.Addresses[i].StreetAddress}, {person.Addresses[i].City}, {person.Addresses[i].State}  {person.Addresses[i].ZipCode}");
				}
			}

			Console.WriteLine();
			bool done = false;
			bool selectionvalid;
			int selection;
			do
			{
				do
				{
					Console.Write("Please select: 1 = Remove an address, 2 = Add an address, 3 = Change an address, 4 = No changes to addresses: ");
					selectionvalid = int.TryParse(Console.ReadLine(), out selection);
					if ( !selectionvalid || selection < 1 || selection > 4 )
					{
						Console.WriteLine("That is not a valid selection.  Please try again.");
					}
				} while ( !selectionvalid || selection < 1 || selection > 4 );

				bool addressvalid;
				int addressselect;
				if ( selection == 1 )
				{
					if ( person.Addresses.Count > 0 )
					{
						Console.Write("Please select the address to remove using the number: ");
						addressvalid = int.TryParse(Console.ReadLine(), out addressselect);
						AddressModel address = person.Addresses[addressselect - 1];
						if ( addressvalid && address != null )
						{
							_ = person.Addresses.Remove(address);
						}
						else
						{
							Console.WriteLine("That was not a valid selection.");
						}
					}
					else
					{
						Console.WriteLine("There are no addresses to remove.");
					}
				}
				else if ( selection == 2 )
				{
					AddressModel address = CreateNewAddress();
					person.Addresses.Add(address);
				}
				else if ( selection == 3 )
				{
					if ( person.Addresses.Count > 0 )
					{
						Console.Write("Please select the address to change using the number: ");
						addressvalid = int.TryParse(Console.ReadLine(), out addressselect);
						AddressModel address = person.Addresses[addressselect - 1];
						if ( addressvalid && address != null )
						{
							address.UpdateAddress();
						}
						else
						{
							Console.WriteLine("That was not a valid selection.");
						}
					}
					else
					{
						Console.WriteLine("There are no addresses to change.");
					}
				}
				else
				{
					done = true;
				}
			} while ( !done );

			if ( person.Employer == null )
			{
				Console.WriteLine("This person has no employer.");
				do
				{
					Console.Write("Please select: 1 = Add an employer, 5 = No change to employer: ");
					selectionvalid = int.TryParse(Console.ReadLine(), out selection);
					if ( !selectionvalid || selection != 1 || selection != 5 )
					{
						Console.WriteLine("That is not a valid selection.  Please try again.");
					}
				} while ( !selectionvalid || selection != 1 || selection != 5 );
			}
			else
			{
				Console.WriteLine("Employer for the Person:");
				Console.WriteLine($"{person.Employer.CompanyName}");
				do
				{
					Console.Write("Please select: 2 = Replace the employer, 3 = Change the employer, 4 = Remove the employer, 5 = No change to employer: ");
					selectionvalid = int.TryParse(Console.ReadLine(), out selection);
					if ( !selectionvalid || selection < 2 || selection > 5 )
					{
						Console.WriteLine("That is not a valid selection.  Please try again.");
					}
				} while ( !selectionvalid || selection < 2 || selection > 4 );
			}

			if ( selection == 2 || selection == 4 )
			{
				person.Employer = null;
			}

			if ( selection == 1 || selection == 2 )
			{
				EmployerModel employer = CreateNewEmployer();
				person.Employer = employer;
			}

			if ( selection == 3 )
			{
				person.Employer.UpdateEmployer();
			}

			_data.UpdatePerson(person);
		}

		private static void UpdateEmployer(this EmployerModel employer)
		{
			Console.WriteLine($"Current Company Name: {employer.CompanyName}");
			Console.Write("Enter new Company Name (press enter on blank line for no change): ");
			string input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				employer.CompanyName = input;
			}

			if ( employer.Addresses.Count == 0 )
			{
				Console.WriteLine("This employer has no addresses.");
			}
			else
			{
				Console.WriteLine("Addresses for the Employer:");
				for ( int i = 0; i < employer.Addresses.Count; i++ )
				{
					Console.WriteLine($"{i + 1}: {employer.Addresses[i].StreetAddress}, {employer.Addresses[i].City}, {employer.Addresses[i].State}  {employer.Addresses[i].ZipCode}");
				}
			}

			Console.WriteLine();
			bool done = false;
			do
			{
				bool selectionvalid;
				int selection;
				do
				{
					Console.Write("Please select: 1 = Remove an address, 2 = Add an address. 3 = Change an address, 4 = No changes to addresses: ");
					selectionvalid = int.TryParse(Console.ReadLine(), out selection);
					if ( !selectionvalid || selection < 1 || selection > 4 )
					{
						Console.WriteLine("That is not a valid selection.  Please try again.");
					}
				} while ( !selectionvalid || selection < 1 || selection > 4 );

				bool addressvalid;
				int addressselect;
				if ( selection == 1 )
				{
					if ( employer.Addresses.Count > 0 )
					{
						Console.Write("Please select the address to remove using the number: ");
						addressvalid = int.TryParse(Console.ReadLine(), out addressselect);
						AddressModel address = employer.Addresses[addressselect - 1];
						if ( addressvalid && address != null )
						{
							_ = employer.Addresses.Remove(address);
						}
						else
						{
							Console.WriteLine("That was not a valid selection.");
						}
					}
					else
					{
						Console.WriteLine("There are no addresses to remove.");
					}
				}
				else if ( selection == 2 )
				{
					AddressModel address = CreateNewAddress();
					employer.Addresses.Add(address);
				}
				else if ( selection == 3 )
				{
					if ( employer.Addresses.Count > 0 )
					{
						Console.Write("Please select the address to change using the number: ");
						addressvalid = int.TryParse(Console.ReadLine(), out addressselect);
						AddressModel address = employer.Addresses[addressselect - 1];
						if ( addressvalid && address != null )
						{
							address.UpdateAddress();
						}
						else
						{
							Console.WriteLine("That was not a valid selection.");
						}
					}
					else
					{
						Console.WriteLine("There are no addresses to change.");
					}
				}
				else
				{
					done = true;
				}
			} while ( !done );
		}

		private static void UpdateAddress(this AddressModel address)
		{
			Console.WriteLine($"Current Street Address: {address.StreetAddress}");
			Console.Write("Enter new Street Address (press enter on blank line for no change): ");
			string input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				address.StreetAddress = input;
			}

			Console.WriteLine($"Current City: {address.City}");
			Console.Write("Enter new City (press enter on blank line for no change): ");
			input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				address.City = input;
			}

			Console.WriteLine($"Current State: {address.State}");
			Console.Write("Enter new State (press enter on blank line for no change): ");
			input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				address.State = input;
			}

			Console.WriteLine($"Current Zip Code: {address.ZipCode}");
			Console.Write("Enter new Zip Code (press enter on blank line for no change): ");
			input = Console.ReadLine();
			if ( !string.IsNullOrWhiteSpace(input) )
			{
				address.ZipCode = input;
			}
		}

		private static void DeletePerson()
		{
			PersonModel output = RetrievePerson();

			_data.DeletePerson(output);
		}

		private static void DisplayPerson(PersonModel person)
		{
			Console.WriteLine();
			Console.WriteLine("-- Person --");
			Console.WriteLine();
			Console.WriteLine($"First Name: {person.FirstName}");
			Console.WriteLine($"Last Name: {person.LastName}");
			Console.WriteLine($"Is Active: {person.IsActive}");
			if ( person.Addresses.Count > 0 )
			{
				Console.WriteLine();
				Console.WriteLine("Addresses for the Person:");
				foreach ( AddressModel address in person.Addresses )
				{
					Console.WriteLine($"{address.StreetAddress}, {address.City}, {address.State}  {address.ZipCode}");
				}
			}

			if ( person.Employer != null )
			{
				Console.WriteLine();
				Console.WriteLine($"Employer: {person.Employer.CompanyName}");
				if ( person.Employer.Addresses.Count > 0 )
				{
					Console.WriteLine();
					Console.WriteLine("Addresses for the Employer:");
					foreach ( AddressModel address in person.Employer.Addresses )
					{
						Console.WriteLine($"{address.StreetAddress}, {address.City}, {address.State}  {address.ZipCode}");
					}
				}
			}

			Console.WriteLine();
		}

		private static void ProgramOutro()
		{
			Console.WriteLine();
			Console.WriteLine("Thank you for using Text File CRUD!");
			Console.WriteLine();
		}
	}
}
