package com.fynzie.news.services;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

import com.fynzie.news.dto.DetailedNews;
import com.fynzie.news.dto.NewsCreation;
import com.fynzie.news.dto.NewsSummary;
import com.fynzie.news.models.News;
import com.fynzie.news.repositories.NewsRepository;
import com.github.slugify.Slugify;

import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 * NewsServiceImpl
 */
@Service
@Transactional
public class NewsServiceImpl implements NewsService
{
    private static final int PAGE_PER_VIEW = 5;

    private final NewsRepository newsRepository;
    private final Slugify slugify;

    public NewsServiceImpl(NewsRepository newsRepository, Slugify slugify)
    {
        this.newsRepository = newsRepository;
        this.slugify = slugify;
    }

    @Override
    public boolean createNews(NewsCreation newsCreation)
    {
        News news = new News(newsCreation.getTitle(), newsCreation.getContent(), newsCreation.getUserCreationId());

        news.setCreationDate(LocalDateTime.now());
        news.setModificationDate(LocalDateTime.now());
        news.setSlug(slugify.slugify(news.getTitle()));

        newsRepository.save(news);

        return true;
    }

    @Override
    public DetailedNews getById(long id)
    {
        return detailedTransform(newsRepository.findById(id));
    }

    @Override
    public DetailedNews getBySlug(String slug)
    {
        return detailedTransform(newsRepository.findBySlug(slug));
    }

    @Override
    public List<NewsSummary> getNews()
    {
        List<News> news = newsRepository.findAll();
        news.sort((a, b) -> {
            return a.getCreationDate().isAfter(b.getCreationDate()) ? -1 : 1;
        });

        if (news.size() <= 5)
        {
            return transform(news);
        }

        return transform(news.subList(0, 5));
    }

    @Override
    public List<NewsSummary> getByOffset(int offset)
    {
        // comments take offset 3 as an example, news[10] -> news[14]
        if (offset <= 1)
        {
            return getNews();
        }

        List<News> news = newsRepository.findAll();
        news.sort((a, b) -> {
            return a.getCreationDate().isAfter(b.getCreationDate()) ? -1 : 1;
        });

        // offset is off the list, 7 < 3 * 5 - 5 (10)
        if (news.size() < offset * PAGE_PER_VIEW - PAGE_PER_VIEW)
        {
            return new ArrayList<NewsSummary>();
        }

        // list is smaller than 14, we need the last x starting from 3 * 5 - 5
        if (news.size() < offset * PAGE_PER_VIEW - 1)
        {
            return transform(news.subList(offset * PAGE_PER_VIEW - PAGE_PER_VIEW, news.size()));
        }

        // if (news.size() <= offset * PAGE_PER_VIEW)
        // {
        //     return transform(news.subList(PAGE_PER_VIEW * (offset - 1), (PAGE_PER_VIEW * (offset - 1)) + news.size() % PAGE_PER_VIEW));
        // }

        // take from 3 * 5 - 5 (10) to 3 * 5 (15 exclusive => 14)
        return transform(news.subList(offset * PAGE_PER_VIEW - PAGE_PER_VIEW, offset * PAGE_PER_VIEW));
    }

    private List<NewsSummary> transform(List<News> news)
    {
        return news.stream().map(x -> transform(x)).collect(Collectors.toList());
    }

    private NewsSummary transform(News news)
    {
        if (news == null)
        {
            return null;
        }

        NewsSummary newsSummary = new NewsSummary();
        newsSummary.setTitle(news.getTitle());
        newsSummary.setSlug(news.getSlug());
        newsSummary.setContent(news.getContent());
        newsSummary.setUserCreationId(news.getUserCreationId());
        newsSummary.setCreationDate(news.getCreationDate());
        newsSummary.setModificationDate(news.getModificationDate());

        return newsSummary;
    }

    private DetailedNews detailedTransform(News news)
    {
        DetailedNews detailedNews = new DetailedNews();
        detailedNews.setId(news.getId());
        detailedNews.setTitle(news.getTitle());
        detailedNews.setSlug(news.getSlug());
        detailedNews.setContent(news.getContent());
        detailedNews.setUserCreationId(news.getUserCreationId());
        detailedNews.setCreationDate(news.getCreationDate());
        detailedNews.setModificationDate(news.getModificationDate());
        detailedNews.setComments(news.getComments());

        return detailedNews;
    }
}