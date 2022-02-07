create database app;

create user webapp with encrypted password 'somePassword123#@!';

create schema crm;

alter schema crm owner to webapp;

create table companies
(
    id                  uuid not null constraint companies_pk primary key,
    domain_name         varchar(255),
    number_of_employees integer,
    active              boolean default false
);

alter table companies
    owner to webapp;

create unique index companies_domain_name_uindex
    on companies (domain_name);

create unique index companies_id_uindex
    on companies (id);

create unique index companies_domain_name_uindex_2
    on companies (domain_name);


create table users
(
    id                 uuid not null constraint users_pk primary key,
    email              varchar(255),
    type               integer,
    is_email_confirmed boolean default false not null
);

alter table users
    owner to webapp;

create unique index users_id_uindex
    on users (id);