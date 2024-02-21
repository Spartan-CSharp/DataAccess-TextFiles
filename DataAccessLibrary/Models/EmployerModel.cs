using System.Collections.Generic;

namespace DataAccessLibrary.Models
{
	public class EmployerModel
	{
		public string CompanyName { get; set; }
		public List<AddressModel> Addresses { get; set; } = new List<AddressModel>();
	}
}
