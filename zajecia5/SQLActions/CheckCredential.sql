create procedure CheckCredential @login varchar(30), @password varchar(30)
as
begin
	declare @isUserExists int = (select IndexNumber from Student where IndexNumber= @login and password = @password);
	if(@isUserExists is null)
	begin
		raiserror('Username or password is wrong',11,1);
		return (@isUserExists);
	end;
	return (@isUserExists);
end;

select * from Enrollment
exec checkCredential 1233, lol123