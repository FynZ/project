package com.fynzie.demo.controllers;
 
import java.util.Date;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
 
@RestController
public class TestController
{
    @RequestMapping(value = "/monsters/{id}")
    public String echoStudentName(@PathVariable(name = "id") int id)
    {
        return "hello  <strong style=\"color: red;\">" + id + " </strong> Responsed on : " + new Date();
    }
}
