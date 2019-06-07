package com.fynzie.news.repositories;

import com.fynzie.news.models.News;

import org.springframework.data.jpa.repository.JpaRepository;

/**
 * NewsRepository
 */
public interface NewsRepository extends JpaRepository<News, Long>
{
    News findById(long id);
    
    News findBySlug(String slug);
}