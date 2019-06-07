package com.fynzie.news.repositories;

import com.fynzie.news.models.News;

import org.springframework.data.jpa.repository.JpaRepository;

/**
 * NewsRepository
 */
public interface NewsRepository extends JpaRepository<News, Long>
{
    public News findById(long id);
    
    public News findBySlug(String slug);
}