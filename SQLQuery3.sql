create table FeeConfiguration(
ID int identity primary key,
TransactionType	varchar(255),
PaymentType	varchar(255),
MinimumCharge decimal(18,2),
PercentageCharge decimal(18,2),
FixedFee decimal(18,2),
Active bit
)
