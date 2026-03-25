/* Used to create the SqlServer database that Schema.fs is generated from */

CREATE TABLE [dbo].[table1]
(
  id [uniqueidentifier] NOT NULL,
  data_value [nvarchar](max) NOT NULL,

  CONSTRAINT pk_table1 PRIMARY KEY (id)
);

CREATE TABLE [dbo].[table2]
(
  table1_id [uniqueidentifier] NOT NULL,
  date [Date] NOT NULL,
  timestamp [DateTime2](7) NOT NULL,

  CONSTRAINT pk_table2 PRIMARY KEY (table1_id, date, timestamp)
);