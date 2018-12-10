namespace POC01.Model.Common
{
    public interface IAddress
    {
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string State { get; set; }
        string ZipCode { get; set; }
        string Country { get; }
    }
}