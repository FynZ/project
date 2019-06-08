-- auto-generated definition
create type role as enum ('User', 'Moderator', 'Developer', 'Admin', 'Owner');

alter type role owner to admin;

