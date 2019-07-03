package com.fynzie.news.controllers;

import org.springframework.security.access.annotation.Secured;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

/**
 * TestController
 */
@RestController
@RequestMapping("/test")
public class TestController extends BaseController
{
    /* TEST AREA */
    @Secured({"ROLE_USER"})
    @GetMapping(value="/user")
    public String testUser()
    {
        int id = this.getUserId();

        return "Hello User with id " + id;
    }

    @Secured({"ROLE_ADMIN"})
    @GetMapping(value="/admin")
    public String testAdmin()
    {
        int id = this.getUserId();

        return "Hello Admin with id " + id;
    }
}