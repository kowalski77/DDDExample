namespace SnackMachine.API.Contracts
{
    public class AddSnackRequest
    {
        public long SnackId { get; set; }

        public int Pile { get; set; }
    }
}