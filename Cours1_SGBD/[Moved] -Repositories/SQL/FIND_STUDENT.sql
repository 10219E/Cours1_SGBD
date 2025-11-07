SELECT *
FROM dbo.Students
WHERE ID = @id
   OR [First Name] LIKE @FirstName
   OR [Last Name] LIKE @LastName;