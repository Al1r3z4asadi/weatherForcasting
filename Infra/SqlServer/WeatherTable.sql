CREATE TABLE Weather (
    Id INT IDENTITY(1,1) PRIMARY KEY, 
    [Key] CHAR(64) NOT NULL,           
    [Data] NVARCHAR(MAX) NOT NULL
);
