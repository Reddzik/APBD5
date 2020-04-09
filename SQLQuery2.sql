create procedure PromoteStudents @semester int, @studiesName varchar(60)
as
begin 
	declare @oldIdEnrollment int =( select IdEnrollment 
									from Studies as s, Enrollment as e
									where e.IdStudy= s.IdStudy and e.Semester = @semester and @studiesName = s.Name);
	declare @newIdEnrollment int =(select IdEnrollment 
									from Studies as s, Enrollment as e
									where e.IdStudy= s.IdStudy and e.Semester = @semester+1 and @studiesName = s.Name);
	if(@newIdEnrollment is null)
	begin
		set @newIdEnrollment = (select MAX(IdEnrollment)+1 from Enrollment);
		declare @studiesId int = (select IdStudy from studies as s where s.name= @studiesName);
		insert into Enrollment(IdEnrollment, Semester, IdStudy, StartDate) values (@newIdEnrollment, @semester+1, @studiesId, GETDATE());
	end;
	update student set IdEnrollment = @newIdEnrollment where IdEnrollment=@oldIdEnrollment;
end;


select * from Enrollment