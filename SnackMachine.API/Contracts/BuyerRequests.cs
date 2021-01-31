namespace SnackMachine.API.Contracts
{
    public record InsertMoneyRequest(decimal Amount);

    public record BuySnackRequest(long Id);
}