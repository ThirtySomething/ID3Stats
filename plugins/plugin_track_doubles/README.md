# Track doubles

List all tracks occuring more than once.

```sql
SELECT
    *
FROM
    id3import
WHERE
    (title, artist) IN(
    SELECT
        title,
        artist
    FROM
        id3import
    GROUP BY
        title,
        artist
    HAVING
        COUNT(title) > 1
    ORDER BY
        title,
        artist
)
ORDER BY
    title,
    artist;
```

## Sample output

An example of the output can be viewed [here][app_statistic].

## Unsolved problems

The program is not able to resolve different namings. For example, in the collection there is a song with the same name but from differnt artists. But the title is written in a different way.

- `argent` - `god gave rock 'n' roll to you`
- `kiss` - `god gave rock & roll to you`

So this song will not listed using the `plugin_track_doubles` statistic.

[app_statistic]: ./../../sample/Titles%20with%20multiple%20artists.html
