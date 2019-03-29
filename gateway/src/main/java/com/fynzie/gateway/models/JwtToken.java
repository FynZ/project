package com.fynzie.gateway.models;

import java.util.List;

import com.fasterxml.jackson.annotation.JsonAlias;

/**
 * JwtToken
 */
public class JwtToken {

    @JsonAlias({ "username", "Username", "userName", "UserName" })
    private String username;

    @JsonAlias({ "roles", "Roles" })
    private List<String> roles;

    /**
     * @return the username
     */
    public String getUsername() {
        return username;
    }

    /**
     * @return the roles
     */
    public List<String> getRoles() {
        return roles;
    }

    /**
     * @param roles the roles to set
     */
    public void setRoles(List<String> roles) {
        this.roles = roles;
    }

    /**
     * @param username the username to set
     */
    public void setUsername(String username) {
        this.username = username;
    }

}