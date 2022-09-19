create table FLAT_TYPE(
	id integer GENERATED ALWAYS AS IDENTITY primary key,
	description varchar
);
insert into public.flat_type (description) values('Собственник');
insert into public.flat_type (description) values('Приватизированная');
insert into public.flat_type (description) values('Госудаственная');

create table FLAT(
	id integer GENERATED ALWAYS AS IDENTITY primary key,
	type_id integer references FLAT_TYPE(id),
	number integer,
	square real
);

create table OWNER(
	id integer GENERATED ALWAYS AS IDENTITY primary key,
	name varchar(100)
);

create table OWNER_FLAT(
	id integer generated always as identity primary key,
	owner_id integer references OWNER(id),
	flat_id integer references FLAT(id),
	square real
);

create table VOTE_TYPE(
	id integer generated always as identity primary key,
	description varchar(30)
);

insert into VOTE_TYPE (description) values ('за');
insert into VOTE_TYPE (description) values ('против');
insert into VOTE_TYPE (description) values ('воздержался');

create table VOTE(
	id integer generated always as identity primary key,
	owner_id integer references OWNER(id),
	type_id integer references VOTE_TYPE(id)
);

alter table public.owner
	add constraint owner_name_unique UNIQUE("name");

create index owner_name_index on public.owner(name);
