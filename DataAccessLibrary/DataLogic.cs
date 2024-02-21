using System;
using System.Collections.Generic;

using DataAccessLibrary.Models;
using DataAccessLibrary.TextFileDataAccess;

using Microsoft.Extensions.Configuration;

namespace DataAccessLibrary
{
	public class DataLogic : IDataLogic
	{
		private readonly IConfiguration _configuration;
		private readonly string _fileName;
		private readonly ICrud _crud;

		public DataLogic(IConfiguration configuration, DBTYPES dbType)
		{
			_configuration = configuration;
			switch ( dbType )
			{
				case DBTYPES.CSVFile:
					_fileName = _configuration.GetValue<string>("CsvFileName");
					_crud = new TextFileCRUD(_fileName);
					break;
				default:
					break;
			}
		}

		public void DeletePerson(PersonModel person)
		{
			_crud.DeletePerson(person);
		}

		public List<PersonModel> GetAllPeople()
		{
			List<PersonModel> output = _crud.RetrieveAllPeople();
			return output;
		}

		public PersonModel GetPersonById(Guid personId)
		{
			PersonModel output = _crud.RetrievePersonById(personId);
			return output;
		}

		public void SaveNewPerson(PersonModel person)
		{
			_crud.CreatePerson(person);
		}

		public void UpdatePerson(PersonModel person)
		{
			_crud.UpdatePerson(person);
		}
	}
}
