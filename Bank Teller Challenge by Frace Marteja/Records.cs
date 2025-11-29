public record Transaction(DateTime timeStamp, TransactionType transactionType, decimal amount);
public record TellerTransaction(DateTime timeStamp, string actions, decimal amount, string accountName);