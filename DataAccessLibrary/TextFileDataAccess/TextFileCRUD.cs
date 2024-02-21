using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using DataAccessLibrary.Models;

namespace DataAccessLibrary.TextFileDataAccess
{
	public class TextFileCRUD : ICrud
	{
		private readonly string _filePath;

		public TextFileCRUD(string fileName)
		{
			string filePath = Directory.GetCurrentDirectory();
			_filePath = filePath + "\\" + fileName;
		}

		public void CreatePerson(PersonModel person)
		{
			List<string> addresses = new List<string>();
			foreach ( AddressModel address in person.Addresses )
			{
				addresses.Add($"{address.StreetAddress}|{address.City}|{address.State}|{address.ZipCode}");
			}

			string addressString = string.Join(";", addresses);
			string employerString = string.Empty;
			if ( person.Employer != null )
			{
				employerString += person.Employer.CompanyName + "^";
				List<string> employerAddresses = new List<string>();
				foreach ( AddressModel address in person.Addresses )
				{
					employerAddresses.Add($"{address.StreetAddress}|{address.City}|{address.State}|{address.ZipCode}");
				}

				employerString += string.Join(";", employerAddresses);
			}

			string personString = $"{person.Id},{person.FirstName},{person.LastName},{person.IsActive},{employerString},{addressString}";

			List<string> existingLines = TextFileDataAccess.ReadAllCSVEntries(_filePath);

			existingLines.Add(personString);

			TextFileDataAccess.WriteAllCSVEntries(_filePath, existingLines);
		}

		public void DeletePerson(PersonModel person)
		{
			List<PersonModel> people = RetrieveAllPeople();

			people.RemoveAt(people.FindIndex(x => x.Id == person.Id));

			List<string> updateList = new List<string>();

			foreach ( PersonModel p in people )
			{
				List<string> addresses = new List<string>();
				foreach ( AddressModel address in p.Addresses )
				{
					addresses.Add($"{address.StreetAddress}|{address.City}|{address.State}|{address.ZipCode}");
				}

				string addressString = string.Join(";", addresses);
				string employerString = string.Empty;
				if ( p.Employer != null )
				{
					employerString += p.Employer.CompanyName + "^";
					List<string> employerAddresses = new List<string>();
					foreach ( AddressModel address in p.Addresses )
					{
						employerAddresses.Add($"{address.StreetAddress}|{address.City}|{address.State}|{address.ZipCode}");
					}

					employerString += string.Join(";", employerAddresses);
				}

				string personString = $"{p.Id},{p.FirstName},{p.LastName},{p.IsActive},{employerString},{addressString}";
				updateList.Add(personString);
			}

			TextFileDataAccess.WriteAllCSVEntries(_filePath, updateList);
		}

		public List<PersonModel> RetrieveAllPeople()
		{
			List<PersonModel> output = new List<PersonModel>();
			List<string> existingLines = TextFileDataAccess.ReadAllCSVEntries(_filePath);

			foreach ( string line in existingLines )
			{
				string[] cols = line.Split(',');
				if ( cols.Length != 6 )
				{
					throw new ApplicationException($"Bad Line in CSV File {_filePath}: {line}");
				}

				PersonModel person = new PersonModel
				{
					Id = new Guid(cols[0]),
					FirstName = cols[1],
					LastName = cols[2],
					IsActive = cols[3].ToLower() == "true"
				};

				if ( cols[4].Length > 0 )
				{
					// split out the employer data
					string[] employerCols = cols[4].Split('^');
					if ( employerCols.Length != 2 )
					{
						throw new ApplicationException($"Bad Employer in CSV File {_filePath} Line {line}: {cols[4]}");
					}

					person.Employer = new EmployerModel
					{
						CompanyName = employerCols[0]
					};

					string[] addresses = employerCols[1].Split(';');
					foreach ( string address in addresses )
					{
						string[] addressCols = address.Split('|');
						if ( addressCols.Length != 4 )
						{
							throw new ApplicationException($"Bad Employer Address in CSV File {_filePath} Line {line}, Employer {cols[4]}: {address}");
						}

						AddressModel addressModel = new AddressModel
						{
							StreetAddress = addressCols[0],
							City = addressCols[1],
							State = addressCols[2],
							ZipCode = addressCols[3]
						};

						person.Employer.Addresses.Add(addressModel);
					}
				}

				if ( cols[5].Length > 0 )
				{
					string[] addresses = cols[5].Split(';');
					foreach ( string address in addresses )
					{
						string[] addressCols = address.Split('|');
						if ( addressCols.Length != 4 )
						{
							throw new ApplicationException($"Bad Address in CSV File {_filePath} Line {line}: {address}");
						}

						AddressModel addressModel = new AddressModel
						{
							StreetAddress = addressCols[0],
							City = addressCols[1],
							State = addressCols[2],
							ZipCode = addressCols[3]
						};

						person.Addresses.Add(addressModel);
					}
				}

				output.Add(person);
			}

			return output;
		}

		public PersonModel RetrievePersonById(Guid id)
		{
			PersonModel output = RetrieveAllPeople().FirstOrDefault(x => x.Id == id);
			return output ?? throw new ApplicationException($"Person with Id of {id} not found in file {_filePath}");
		}

		public void UpdatePerson(PersonModel person)
		{
			List<PersonModel> people = RetrieveAllPeople();

			PersonModel personToUpdate = people.FirstOrDefault(x => x.Id == person.Id) ?? throw new ApplicationException($"Person with Id of {person.Id} not found in file {_filePath}");

			personToUpdate = person;

			List<string> updateList = new List<string>();

			foreach ( PersonModel p in people )
			{
				List<string> addresses = new List<string>();
				foreach ( AddressModel address in p.Addresses )
				{
					addresses.Add($"{address.StreetAddress}|{address.City}|{address.State}|{address.ZipCode}");
				}

				string addressString = string.Join(";", addresses);
				string employerString = string.Empty;
				if ( p.Employer != null )
				{
					employerString += p.Employer.CompanyName + "^";
					List<string> employerAddresses = new List<string>();
					foreach ( AddressModel address in p.Addresses )
					{
						employerAddresses.Add($"{address.StreetAddress}|{address.City}|{address.State}|{address.ZipCode}");
					}

					employerString += string.Join(";", employerAddresses);
				}

				string personString = $"{p.Id},{p.FirstName},{p.LastName},{p.IsActive},{employerString},{addressString}";
				updateList.Add(personString);
			}

			TextFileDataAccess.WriteAllCSVEntries(_filePath, updateList);
		}
	}
}
