using System;
using System.Collections.Generic;
using System.Text;

using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
	public interface ICrud
	{
		void CreatePerson(PersonModel person);
		List<PersonModel> RetrieveAllPeople();
		PersonModel RetrievePersonById(Guid id);
		void UpdatePerson(PersonModel person);
		void DeletePerson(PersonModel person);
	}
}
