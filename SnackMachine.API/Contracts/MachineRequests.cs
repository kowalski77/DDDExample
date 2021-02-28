namespace SnackMachine.API.Contracts
{
    public record RegisterMachineRequest(string Name);

    public record AddSnackRequest(long SnackId, int Pile);
}