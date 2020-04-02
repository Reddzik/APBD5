create procedure EnrollStudent @Studies nvarchar(100), @indexNumber nvarchar(30), @FirstName nvarchar(15), @LastName nvarchar(60),
@BirthDate Date
as
begin
	begin tran

	declare @errMessage nvarchar(255)
	declare @IdStudies int = (select IdStudy from studies where name=@Studies);
	declare @DoesStudentNumberExist int = (select count(*) from Student where IndexNumber=@indexNumber);
	if @DoesStudentNumberExist != 0
	begin
		set @errMessage ='Student o takim ID ju¿ istnieje';
		raiserror(@errMessage, 11,1);
		rollback;
		return;
	end;
	if @IdStudies is null
	begin
		set @errMessage = 'Nie ma takich studiów';
		raiserror(@errMessage, 11,1);
		rollback;
		return;
	end;
	declare @IdEnrollment int = (select top 1 IdEnrollment
								from Enrollment where Semester=1 and IdStudy=@IdStudies
								order by StartDate desc);
	if @IdEnrollment is null
	begin
		declare @currentDate Date = (SELECT CONVERT(char(10), GetDate(),126));
		declare @maxIdEnrollment int = (select max(IdEnrollment) from Enrollment);
		insert into Enrollment (IdEnrollment, Semester,IdStudy,StartDate)
		values ((@maxIdEnrollment+1), 1, @IdStudies, @currentDate);
		Print 'Dodano element';
		set @IdEnrollment = @maxIdEnrollment +1;
	end;
	insert into Student (IndexNumber,FirstName,LastName,BirthDate,IdEnrollment) 
	values (@indexNumber, @FirstName, @LastName, @BirthDate, @IdEnrollment);
	commit;
end;

		
exec EnrollStudent 'PL', '12334', 'Mateusz', 'Pawlak', '1998-03-03';

select s.FirstName, e.Semester from student as s join Enrollment as e on s.IdEnrollment = e.IdEnrollment where s.IndexNumber='12334';