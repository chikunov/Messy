CREATE TABLE [$SchemaName$].[Events]
(
	[AggregateId] [uniqueidentifier] NOT NULL,
	[AggregateName] [nvarchar](255) NOT NULL,
	[Version] [int] NOT NULL,
	[DateTime] [datetime2] NOT NULL,
	[EventName] [nvarchar](255) NOT NULL,
	[Data] [varbinary](max) NOT NULL
    CONSTRAINT [PK_$SchemaName$.Events] PRIMARY KEY CLUSTERED ([AggregateId], [Version])
);