create view AttendanceView
as
select a.Id,a.Day,a.SignInTime,a.SignOutTime,a.AttendenceStatus,u.UserName,a.UserId  
from TblAttendance a inner join AspNetUsers U 
on a.UserId=u.Id