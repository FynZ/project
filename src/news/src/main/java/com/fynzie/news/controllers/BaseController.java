package com.fynzie.news.controllers;

import org.springframework.security.core.context.SecurityContextHolder;

/**
 * BaseController
 */
public abstract class BaseController
{
    public int getUserId() throws NumberFormatException
    {
        Object obj = SecurityContextHolder.getContext().getAuthentication().getPrincipal();

        return Integer.parseInt((String)SecurityContextHolder.getContext().getAuthentication().getPrincipal());
    }    
}