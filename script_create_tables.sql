CREATE TABLE DB_A36660_FeiraPreta.dbo.EventScore (
	id uniqueidentifier(36) NOT NULL,
	value float NOT NULL,
	createdDate datetime2 NOT NULL,
	CONSTRAINT PK_EventScore PRIMARY KEY (id)
) go;

CREATE TABLE DB_A36660_FeiraPreta.dbo.Person (
	id uniqueidentifier(36) NOT NULL,
	usernameInstagram varchar(50) NOT NULL,
	fullNameInstagram varchar(200) NOT NULL,
	createdDate datetime2 NOT NULL,
	updatedDate datetime2,
	profilePictureInstagram text NOT NULL,
	phoneNumber varchar(30) NOT NULL,
	deletedDate datetime,
	CONSTRAINT PK_Person PRIMARY KEY (id)
) go;

CREATE TABLE DB_A36660_FeiraPreta.dbo.Publication (
	id uniqueidentifier(36) NOT NULL,
	personId uniqueidentifier(36) NOT NULL,
	link varchar(100) NOT NULL,
	createdDateInstagram datetime2 NOT NULL,
	createdDate datetime2 NOT NULL,
	updatedDate datetime2,
	isHighlight bit NOT NULL,
	imageLowResolution text NOT NULL,
	imageThumbnail text NOT NULL,
	imageStandardResolution text NOT NULL,
	subtitle text,
	deletedDate datetime2,
	CONSTRAINT PK_Publication PRIMARY KEY (id),
	CONSTRAINT FK_Publication_Person FOREIGN KEY (personId) REFERENCES DB_A36660_FeiraPreta.dbo.Person(id) ON DELETE CASCADE ON UPDATE RESTRICT
) go;

CREATE TABLE DB_A36660_FeiraPreta.dbo.Publication_Tag (
	publicationId uniqueidentifier(36) NOT NULL,
	tagId uniqueidentifier(36) NOT NULL,
	CONSTRAINT FK_Publication_Tag_Publication FOREIGN KEY (publicationId) REFERENCES DB_A36660_FeiraPreta.dbo.Publication(id) ON DELETE CASCADE ON UPDATE RESTRICT,
	CONSTRAINT FK_Publication_Tag_Tag FOREIGN KEY (tagId) REFERENCES DB_A36660_FeiraPreta.dbo.Tag(id) ON DELETE RESTRICT ON UPDATE RESTRICT
) go;

CREATE TABLE DB_A36660_FeiraPreta.dbo.Tag (
	id uniqueidentifier(36) NOT NULL,
	nome varchar(200) NOT NULL,
	CONSTRAINT PK_Tag PRIMARY KEY (id)
) go;
