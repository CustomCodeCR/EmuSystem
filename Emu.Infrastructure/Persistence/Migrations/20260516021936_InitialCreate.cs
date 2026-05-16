using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Emu.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    slug = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tenant", x => x.tenant_id);
                });

            migrationBuilder.CreateTable(
                name: "ApiKey",
                columns: table => new
                {
                    api_key_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    key_hash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    key_prefix = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_used_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_api_key", x => x.api_key_id);
                    table.ForeignKey(
                        name: "fk_api_key_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "Tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    audit_log_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actor_type = table.Column<short>(type: "smallint", maxLength: 50, nullable: false),
                    actor_id = table.Column<Guid>(type: "uuid", nullable: true),
                    action = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    resource_type = table.Column<short>(type: "smallint", maxLength: 50, nullable: false),
                    resource_id = table.Column<Guid>(type: "uuid", nullable: true),
                    path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ip_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    user_agent = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_log", x => x.audit_log_id);
                    table.ForeignKey(
                        name: "fk_audit_log_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "Tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    slug = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project", x => x.project_id);
                    table.ForeignKey(
                        name: "fk_project_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "Tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_login_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_user_tenant_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "Tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectEnvironment",
                columns: table => new
                {
                    environment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    slug = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project_environment", x => x.environment_id);
                    table.ForeignKey(
                        name: "fk_project_environment_project_project_id",
                        column: x => x.project_id,
                        principalTable: "Project",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccessPolicy",
                columns: table => new
                {
                    access_policy_id = table.Column<Guid>(type: "uuid", nullable: false),
                    api_key_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: true),
                    environment_id = table.Column<Guid>(type: "uuid", nullable: true),
                    path_prefix = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    can_read = table.Column<bool>(type: "boolean", nullable: false),
                    can_write = table.Column<bool>(type: "boolean", nullable: false),
                    can_delete = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_access_policy", x => x.access_policy_id);
                    table.ForeignKey(
                        name: "fk_access_policy_api_keys_api_key_id",
                        column: x => x.api_key_id,
                        principalTable: "ApiKey",
                        principalColumn: "api_key_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_access_policy_project_environment_environment_id",
                        column: x => x.environment_id,
                        principalTable: "ProjectEnvironment",
                        principalColumn: "environment_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_access_policy_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "Project",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_access_policy_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "Tenant",
                        principalColumn: "tenant_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Secret",
                columns: table => new
                {
                    secret_id = table.Column<Guid>(type: "uuid", nullable: false),
                    environment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    current_version_number = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<short>(type: "smallint", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_secret", x => x.secret_id);
                    table.ForeignKey(
                        name: "fk_secret_project_environment_environment_id",
                        column: x => x.environment_id,
                        principalTable: "ProjectEnvironment",
                        principalColumn: "environment_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SecretVersion",
                columns: table => new
                {
                    secret_version_id = table.Column<Guid>(type: "uuid", nullable: false),
                    secret_id = table.Column<Guid>(type: "uuid", nullable: false),
                    version_number = table.Column<int>(type: "integer", nullable: false),
                    encrypted_value = table.Column<string>(type: "text", nullable: false),
                    nonce = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    tag = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    algorithm = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_secret_version", x => x.secret_version_id);
                    table.ForeignKey(
                        name: "fk_secret_version_secret_secret_id",
                        column: x => x.secret_id,
                        principalTable: "Secret",
                        principalColumn: "secret_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_access_policy_environment_id",
                table: "AccessPolicy",
                column: "environment_id");

            migrationBuilder.CreateIndex(
                name: "ix_access_policy_project_id",
                table: "AccessPolicy",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPolicy_ApiKeyId",
                table: "AccessPolicy",
                column: "api_key_id");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPolicy_ApiKeyId_PathPrefix",
                table: "AccessPolicy",
                columns: new[] { "api_key_id", "path_prefix" });

            migrationBuilder.CreateIndex(
                name: "IX_AccessPolicy_IsActive",
                table: "AccessPolicy",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPolicy_TenantId",
                table: "AccessPolicy",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKey_KeyHash",
                table: "ApiKey",
                column: "key_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApiKey_KeyPrefix",
                table: "ApiKey",
                column: "key_prefix");

            migrationBuilder.CreateIndex(
                name: "IX_ApiKey_TenantId",
                table: "ApiKey",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_CreatedAt",
                table: "AuditLog",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_ResourseType_ResourseId",
                table: "AuditLog",
                columns: new[] { "resource_type", "resource_id" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_TenantId",
                table: "AuditLog",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_TenantId_CreatedAt",
                table: "AuditLog",
                columns: new[] { "tenant_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "IX_Project_IsActive",
                table: "Project",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_Project_TenantId",
                table: "Project",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Project_TenantId_Slug",
                table: "Project",
                columns: new[] { "tenant_id", "slug" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEnvironment_ProjectId",
                table: "ProjectEnvironment",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEnvironment_ProjectId_Slug",
                table: "ProjectEnvironment",
                columns: new[] { "project_id", "slug" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Secret_EnvironmentId",
                table: "Secret",
                column: "environment_id");

            migrationBuilder.CreateIndex(
                name: "IX_Secret_EnvironmentId_Path",
                table: "Secret",
                columns: new[] { "environment_id", "path" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Secret_Status",
                table: "Secret",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_SecretVersion_SecretId",
                table: "SecretVersion",
                column: "secret_id");

            migrationBuilder.CreateIndex(
                name: "IX_SecretVersion_SecretId_VersionNumber",
                table: "SecretVersion",
                columns: new[] { "secret_id", "version_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_IsActive",
                table: "Tenant",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_Slug",
                table: "Tenant",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_tenant_id",
                table: "User",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_tenant_id_email",
                table: "User",
                columns: new[] { "tenant_id", "email" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessPolicy");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "SecretVersion");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ApiKey");

            migrationBuilder.DropTable(
                name: "Secret");

            migrationBuilder.DropTable(
                name: "ProjectEnvironment");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Tenant");
        }
    }
}
