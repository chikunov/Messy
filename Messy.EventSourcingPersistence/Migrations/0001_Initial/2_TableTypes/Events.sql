CREATE TYPE [$SchemaName$].[EventsTableType] AS TABLE 
(
	[AggregateId] [uniqueidentifier] NOT NULL,
	[AggregateName] [nvarchar](255) NOT NULL,
	[Version] [int] NOT NULL,
	[DateTime] [datetime2] NOT NULL,
	[EventName] [nvarchar](255) NOT NULL,
	[Data] [varbinary](max) NOT NULL
);