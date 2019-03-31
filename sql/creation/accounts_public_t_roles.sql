create table t_roles
(
  id   serial      not null
    constraint t_roles_pk
      primary key,
  name varchar(50) not null
);

alter table t_roles
  owner to admin;

create unique index t_roles_id_uindex
  on t_roles (id);

create unique index t_roles_name_uindex
  on t_roles (name);


create table t_user_roles
(
  id      serial  not null
    constraint t_user_roles_pk
      primary key,
  user_id integer not null,
  role_id integer not null
);

alter table t_user_roles
  owner to admin;

create unique index t_user_roles_id_uindex
  on t_user_roles (id);


create table t_users
(
  id             serial                not null
    constraint t_users_pk
      primary key,
  username       varchar(50)           not null,
  username_upper varchar(50)           not null,
  email          varchar(255)          not null,
  email_upper    varchar(255)          not null,
  password       varchar(255)          not null,
  verified       boolean default false not null,
  banned         boolean default false not null
);

alter table t_users
  owner to admin;

create unique index t_users_email_upper_uindex
  on t_users (email_upper);

create unique index t_users_id_uindex
  on t_users (id);

create unique index t_users_username_upper_uindex
  on t_users (username_upper);

