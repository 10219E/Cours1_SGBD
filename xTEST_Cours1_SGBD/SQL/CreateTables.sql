/****** Object:  Table [dbo].[Students]    Script Date: 20-11-25 00:18:04 ******/
USE [xTEST_SGBD_C];
CREATE TABLE [dbo].[Students](
	[id] [int] IDENTITY(111,1) NOT NULL,
	[First Name] [nvarchar](50) NOT NULL,
	[Last Name] [nvarchar](50) NOT NULL,
	[E-mail] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Section] [nvarchar](50) NOT NULL,
	[Confirmed] [date] NOT NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
