package com.fynzie.news.controllers;

import org.springframework.security.core.context.SecurityContextHolder;

/**
 * BaseController
 */
public abstract class BaseController
{
    public int getUserId() throws NumberFormatException
    {
        return Integer.parseInt((String)SecurityContextHolder.getContext().getAuthentication().getPrincipal());
    }    
}