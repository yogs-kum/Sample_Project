USE [master]
GO
/****** Object:  Database [Codis]    Script Date: 8/12/2019 12:33:36 PM ******/
CREATE DATABASE [Codis] ON  PRIMARY 
( NAME = N'Codis', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\Codis.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Codis_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\Codis_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Codis] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Codis].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Codis] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Codis] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Codis] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Codis] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Codis] SET ARITHABORT OFF 
GO
ALTER DATABASE [Codis] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Codis] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Codis] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Codis] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Codis] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Codis] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Codis] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Codis] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Codis] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Codis] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Codis] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Codis] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Codis] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Codis] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Codis] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Codis] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Codis] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Codis] SET RECOVERY FULL 
GO
ALTER DATABASE [Codis] SET  MULTI_USER 
GO
ALTER DATABASE [Codis] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Codis] SET DB_CHAINING OFF 
GO
USE [Codis]
GO
/****** Object:  Table [dbo].[Addresses]    Script Date: 8/12/2019 12:33:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Addresses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[Line1] [nvarchar](max) NULL,
	[Line2] [ntext] NULL,
	[Country] [ntext] NULL,
	[Postcode] [varchar](50) NULL,
 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CountryMaster]    Script Date: 8/12/2019 12:33:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CountryMaster](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[People]    Script Date: 8/12/2019 12:33:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[DOB] [smalldatetime] NULL,
	[NickName] [text] NULL,
 CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Addresses]  WITH CHECK ADD  CONSTRAINT [FK_Addresses_People] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([ID])
GO
ALTER TABLE [dbo].[Addresses] CHECK CONSTRAINT [FK_Addresses_People]
GO
/****** Object:  StoredProcedure [dbo].[AddEditAddress]    Script Date: 8/12/2019 12:33:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddEditAddress] 
	-- Add the parameters for the stored procedure here
	@Id int = null,
	@Pid int , 
	@Line1 nvarchar(Max),
	@Line2 nvarchar(MAX),
	@Country varchar(100),
	@Postcode nvarchar(Max)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

    -- Insert statements for procedure here
	if (@Id is null)
	begin
	insert into Addresses(Line1,Line2,Country,Postcode,PersonId) values (@Line1,@Line2,@Country,@Postcode,@Pid)
	
	end
	else
	begin
	update Addresses set Line1= @Line1 , Line2= Line2,Country= @Country, Postcode= @Postcode
	where ID = @Id 
	end

	select SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[AddEditPerson]    Script Date: 8/12/2019 12:33:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddEditPerson] 
	-- Add the parameters for the stored procedure here
	@Pid int = null, 
	@FirstName nvarchar(Max),
	@LastName nvarchar(MAX),
	@DOB smalldatetime,
	@NickName nvarchar(Max)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

    -- Insert statements for procedure here
	if (@Pid is null)
	begin
	insert into People(FirstName,LastName,DOB,NickName) values (@FirstName,@LastName,@DOB,@NickName)
	
	end
	else
	begin
	update People set FirstName= @FirstName , LastName= @LastName,DOB= @DOB, NickName= @NickName
	where ID = @Pid 
	end

	select SCOPE_IDENTITY()
END

GO
/****** Object:  StoredProcedure [dbo].[DeleteAddress]    Script Date: 8/12/2019 12:33:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAddress] 
	-- Add the parameters for the stored procedure here
	@ID int
	

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;
	
    -- Insert statements for procedure here
	
	

	Delete from Addresses where Id= @ID
	
	
	
	

	
	
	
	
	
END



GO
/****** Object:  StoredProcedure [dbo].[DeletePerson]    Script Date: 8/12/2019 12:33:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeletePerson] 
	-- Add the parameters for the stored procedure here
	@Pid int
	

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;
	
    -- Insert statements for procedure here
	Begin Transaction
	begin try

	Delete from Addresses where PersonId= @Pid 
	
	Delete from People where ID= @Pid
	
	commit transaction

	end try 
	begin catch 
	rollback transaction
	end catch 
	
	
END


GO
/****** Object:  StoredProcedure [dbo].[FillAllAddresses]    Script Date: 8/12/2019 12:33:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FillAllAddresses]
@pID int=null
--@Lname varchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Line1 as 'Line 1',Line2 as 'Line 2',Country as 'Country',PostCode as 'PIN',ID,PersonId   from Addresses
	where  PersonId = case when  @pID is not null then   @pID else PersonId end
	
END


GO
/****** Object:  StoredProcedure [dbo].[FillAllCountries]    Script Date: 8/12/2019 12:33:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FillAllCountries]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Select CountryID,CountryName from CountryMaster
	
END


GO
/****** Object:  StoredProcedure [dbo].[FillAllNames]    Script Date: 8/12/2019 12:33:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FillAllNames]
@Fname varchar(500)=null, 
@Lname varchar(500)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT FirstName as 'First Name',LastName as 'Last Name',Convert(varchar(100),DOB,103) as 'Birth Date',NickName as 'Nick Name',ID  from People   
	where  FirstName= case when @Fname is not null then  @Fname else FirstName end 
	and LastName = case when @Lname is not null then @Lname else LastName end  
	
END

GO
USE [master]
GO
ALTER DATABASE [Codis] SET  READ_WRITE 
GO
