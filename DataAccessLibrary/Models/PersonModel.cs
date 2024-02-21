using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
	public class PersonModel
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsActive { get; set; }
		public EmployerModel? Employer { get; set; }
		public List<AddressModel> Addresses { get; set; } = new List<AddressModel>();
	}
}
