create procedure addRefreshToken @token varchar(255), @index varchar(50)
as
begin
	begin tran;
	declare @studentToUpdate varchar(50) = (select IndexNumber from Student where IndexNumber= @index)
	if(@studentToUpdate=null)
	begin
		raiserror('Nie ma studenta o takim indexie', 1,11);
		rollback
	end;
	update Student
	set RefreshToken = @token
	where IndexNumber = @studentToUpdate;
	commit;
end;
