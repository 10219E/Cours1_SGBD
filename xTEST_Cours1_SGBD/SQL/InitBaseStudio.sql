USE [xTEST_SGBD_C];
SET IDENTITY_INSERT [dbo].[Studio] ON;

DELETE FROM [dbo].[Studio];

INSERT INTO [dbo].[Studio] (
  [UID],
  [No],
  [Building],
  [Lease Renewal],
  [Student]
) VALUES
(1000, 101, 'Bolshoi',      NULL, NULL),
(1001, 102, 'Bolshoi',      NULL, NULL),
(1005, 103, 'Bolshoi',      '2026-08-31', 6),
(1006, 101, 'Little Italy', NULL, NULL),
(1007, 102, 'Little Italy', '2026-12-15', 4),
(1009, 101, 'Nobel',        NULL, 9),
(1016, 302, 'Bolshoi',      NULL, 2);

SET IDENTITY_INSERT [dbo].[Studio] OFF;