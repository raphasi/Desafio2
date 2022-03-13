
drop table students
drop table courses
drop table subjects

create table Subjects
( Code  varchar(10) primary key,
  Title varchar(30) not null
);

insert into subjects values('MS_NET45','Microsoft.Net 4.5');
insert into subjects values('JAVASE8','Java SE 8.0');
insert into subjects values('ORACLE12C','Oracle Database 12c');

create table Courses 
( [Code]        int identity primary key,
  [SubjectCode] varchar(10)  references subjects(code),
  [StartDate]   datetime not null,
  [EndDate]     datetime,
  [Timing]      varchar(15),
  [CourseFee]   int,
  [Remarks]     varchar(1000),
  [CourseName]  varchar(50) 
);


insert into Courses values('MS_NET45','7/15/2014',null,'08:00-10:00',6000,'');
insert into Courses values('JAVASE8','8/26/2014',null,'07:00-08:00',3000,'');


create table Students 
( [Id]         int  identity primary key,
  [CourseCode] int  references Courses(code) on delete cascade,
  [RollNo]     int not null,
  [Fullname]   varchar(30),
  [Email]      varchar(50),
  [Mobile]     varchar(10),
  [Occupation] varchar(200),
  [Qualification] varchar(200),
  [JoinedOn]   datetime,
  [FeePaid]    int,
  [Remarks]    varchar(1000) 
);



  