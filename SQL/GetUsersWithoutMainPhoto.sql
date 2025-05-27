CREATE PROCEDURE GetUsersWithoutMainPhoto()

BEGIN

    select u.UserName

    from aspnetusers u

    where NOT EXISTS (

            SELECT 1 FROM photos p
            WHERE p.AppUserId = u.Id AND p.IsMain = 1
        );

end


GRANT EXECUTE ON PROCEDURE GetUsersWithoutMainPhoto TO 'admin'@'%';

call GetUsersWithoutMainPhoto();