-- auto-generated definition
create table t_users
(
    id              serial                not null
        constraint t_users_pk
            primary key,
    username        varchar(50)           not null,
    username_upper  varchar(50)           not null,
    email           varchar(255)          not null,
    email_upper     varchar(255)          not null,
    password        varchar(255)          not null,
    server          server                not null,
    in_game_name    varchar(30)           not null,
    subscribed      boolean default false not null,
    verified        boolean default false not null,
    banned          boolean default false not null,
    last_login_date timestamp             not null
);

alter table t_users
    owner to admin;

create unique index t_users_email_upper_uindex
    on t_users (email_upper);

create unique index t_users_id_uindex
    on t_users (id);

create unique index t_users_username_upper_uindex
    on t_users (username_upper);

