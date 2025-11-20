USE [xTEST_SGBD_C];
SET IDENTITY_INSERT [dbo].[Students] ON;

DELETE FROM [dbo].[Students];

INSERT INTO [dbo].[Students] (
  [id],
  [First Name],
  [Last Name],
  [E-mail],
  [Mobile],
  [Section],
  [Confirmed]
) VALUES
(2, 'Paul',  'Mlkv',  'pmlkv@ephec.be', '+32499887711166', 'IT4', '2025-09-12'),
(3, 'Robert','Mark',  'rmark@ephec.be', NULL,                'IT2', '2025-09-16'),
(4, 'Carla', 'Robert','crobert@ephec.be','661562',           'MA3', '2025-09-16'),
(6, 'Bayne', 'Karl',  'kbayne@ep.com',   '+3200292',         'MA1', '2025-10-03'),
(9, 'Lisa',  'Mocha', 'lmocha@ep.com',  '+32002921111',     'MA1', '2025-10-03');

SET IDENTITY_INSERT [dbo].[Students] OFF;