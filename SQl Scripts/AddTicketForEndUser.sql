print 'AddTicketForEndUser'
if exists (select name from sysobjects where name = 'AddTicketForEndUser')
  drop procedure AddTicketForEndUser
go

create procedure AddTicketForEndUser
               ( @IdNo          varchar(13),
			           @TicketClassId int,
				         @TicketPrice   int)
as

declare @EndUserId int

select @EndUserId = ID
  from Enduser
 where IDNumber = @IdNo

insert ticket 
     ( TicketNumber,
       EndUserID,
       StatusID,
       TicketPurchasePrice,
       TicketClassID,
       DatetimePurchased,
       DatetimeReserved,
       DatetimeRedeemed,
       Hash,
       DateTimeUpdated,
       UniquePaymentID)
values(substring(convert(varchar(50),newid()),0,10),
       @EndUserId,
	     5, --purchased
	     @TicketPrice,
	     @TicketClassId,
	     getdate(),
	     null,
	     null,
	     newid(),
	     getdate(),
	     null)

update ticketclass
   set RunningQuantity = RunningQuantity - 1
 where ID = @TicketClassId