namespace HospitalOPD.Api.Helpers
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }
    }
}
