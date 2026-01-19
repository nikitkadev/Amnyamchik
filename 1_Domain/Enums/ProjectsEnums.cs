namespace Amnyam._1_Domain.Enums;

public enum ErrorCodes
{
    NO_ERROR = 0,
    INTERNAL_ERROR = 1,
    NOT_FOUND = 2,
    VARIABLE_IS_NULL = 3,
    ROLE_ASSIGNMENT_FAILED = 4,
    ROLE_REMOVAL_FAILED = 5,
    DB_ERROR = 6,
}

public enum RoleType
{
    SERVER,
    CATEGORY,
    UNIQUE,
    COLOR
}
