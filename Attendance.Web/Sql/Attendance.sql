
select COUNT(*) from TblAttendance 
select * from TblAttendance 

select * from TblLeaveUserModel 
go
Create Procedure GetAttendanceForLastWeek
as
select DATENAME(WEEKDAY,Day(Day)) 
as DayName ,COUNT(*) AttendanceCount 
from TblAttendance 
where AttendenceStatus=0 
and day>getdate()-7
group by DAY(Day) 

go

select getdate()-7

insert into  TblAttendance values(

GETDATE(),GETDATE()-6,GETDATE()-6,	0	,'bee2a92c-1d1f-49c2-8881-e44f256a1e65',GETDATE())


go

Create Procedure GetLeaveRequestsForLastWeek
as
select DATENAME(WEEKDAY,Day(DateCreated)) 
as DayName,DateCreated,COUNT(*) LeaveRequestsCount 
from TblLeaveUserModel 
where DateCreated>getdate()-7
group by DAY(DateCreated) ,DateCreated

go
create view AttendanceView
as
select a.Id,a.Day,a.SignInTime,a.SignOutTime,a.AttendenceStatus,u.UserName,a.UserId  
from TblAttendance a inner join AspNetUsers U 
on a.UserId=u.Id
 select * from TblAttendance
 go
 Create view ITSCAttendanceCounters
as
select 
(select COUNT(*) from TblAttendance where   DATENAME(DAY,[Day])= DATENAME(DAY,GETDATE())and AttendenceStatus=0 ) as TodayAttendenceCount,
(select COUNT(*) from TblLeaveTypeModel ) as LeaveTypeCount,
(select COUNT(*) from TblUserContractModel where GETDATE() between ContractStartDate and ContractEndDate ) as ActiveUserContracts,
(select COUNT(*) from  AspNetUsers au inner join AspNetUserClaims ac on au.Id=ac.UserId WHERE  ac.ClaimType='Permission' and ClaimValue='StaffMember'  ) as StaffMembersCount,
(select COUNT(*) from TblContractType ) as ContractTypeCount


select * from TblUserContractModel
select * from TblContractType
select * from TblUserContractModel

select * from AspNetUsers