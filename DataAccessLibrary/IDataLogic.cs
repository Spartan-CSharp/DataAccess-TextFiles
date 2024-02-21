using System;
using System.Collections.Generic;
using System.Text;

using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
	public interface IDataLogic
	{
		void DeletePerson(PersonModel person);
		List<PersonModel> GetAllPeople();
		PersonModel GetPersonById(Guid personId);
		void SaveNewPerson(PersonModel person);
		void UpdatePerson(PersonModel person);
	}
}
