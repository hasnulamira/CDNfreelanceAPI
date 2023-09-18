using CDNAPI.freelanceCDNModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CDNAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public object GetAllUser(Int32 PageNum, Int32 PageSize)
        {
            freelanceCDNContext context = new();

            try
            {
                Users userList = new Users();

                var total = context.Users.Count();
                var users = context.Users.Skip(((PageNum) * PageSize)).Take(PageSize).ToList();

                if (users.Count > 0)
                {
                    userList.TotalRecord = total;
                    foreach (var item in users)
                    {
                        User us = new User();
                        us.UserID = (int)item.UserId;
                        us.Username = item.UserName;
                        us.PhoneNum = item.UserPhoneNum;
                        us.Email = item.UserEmail;

                        var skill = context.Skills.Where(s => s.UserId == item.UserId).Select(d => new Skills
                        {
                            SkillID = (int)d.SkillId,
                            SkillName = d.SkillName
                        }).ToList();

                        if (skill.Any())
                            us.Skill = skill;

                        var hoobies = context.Hobbies.Where(s => s.UserId == item.UserId).Select(d => new Hobbies
                        {
                            HobbyID = (int)d.HobbyId,
                            HobbyName = d.HobbyName
                        }).ToList();

                        if (hoobies.Count > 0)
                            us.Hobby = hoobies;

                        userList.User.Add(us);
                    }
                }
                else
                {
                    return new Err { HasError = true, ErrMsg = "No Record" };
                }
                    

                return userList;
            }
            catch (Exception ex)
            {
                return new Err { HasError = true, ErrMsg = ex.Message };
            }

        }
        [HttpGet]
        public object GetUser(Int32 ID)
        {
            freelanceCDNContext context = new();
            
            try
            {
                User userList = new User();
                var users = context.Users
                            .Where(u => u.UserId == ID).FirstOrDefault();
                if(users == null)
                    return new Err { HasError = true, ErrMsg = "No Record" };

                userList.UserID = (int)users.UserId;
                userList.Username = users.UserName;
                userList.Email = users.UserEmail;
                userList.PhoneNum = users.UserPhoneNum;

                var skills = context.Skills.Where(s => s.UserId == users.UserId).Select(d => new Skills
                {
                    SkillID = (int)d.SkillId,
                    SkillName = d.SkillName
                }).ToList();

                if (skills.Count > 0)
                    userList.Skill = skills;

                var hoobies = context.Hobbies.Where(s => s.UserId == users.UserId).Select(d => new Hobbies
                {
                    HobbyID = (int)d.HobbyId,
                    HobbyName = d.HobbyName
                }).ToList();

                if (hoobies.Count > 0)
                    userList.Hobby = hoobies;


                return userList;
            }
            catch (Exception ex)
            {
                return new Err { HasError = true, ErrMsg = ex.Message };
            }
            
        }
        [HttpPost]
        public async Task<object> RegisterUser([FromBody] RegisterParams Params)
        {
            freelanceCDNContext context = new();
            var dbTransaction = context.Database.BeginTransaction();
            try
            {
                dbTransaction.CreateSavepoint("BeforeInsertUser");
                var exist = context.Users.Where(u => u.UserName == Params.Username).FirstOrDefault();
                if (exist != null)
                    return new Err { HasError = true, ErrMsg = "Username already exist" };

                var newUser = new freelanceCDNModel.User
                {
                    UserName = Params.Username,
                    UserEmail = Params.Email,
                    UserPhoneNum = Params.PhoneNum
                };
                context.Add(newUser);
                if(await context.SaveChangesAsync() > 0)
                {
                    if (Params.Skill.Count > 0)
                    {
                        foreach (var item in Params.Skill)
                        {
                            var newSkill = new freelanceCDNModel.Skill
                            {
                                UserId = newUser.UserId,
                                SkillName = item.SkillName
                            };
                            context.Add(newSkill);
                        }
                        if (await context.SaveChangesAsync() <= 0)
                        {
                            dbTransaction.RollbackToSavepoint("BeforeInsertUser");
                            return new Err { HasError = true, ErrMsg = "Register failed" };
                        }
                    }

                    if (Params.Hobby.Count > 0)
                    {
                        foreach (var item in Params.Hobby)
                        {
                            var newSkill = new freelanceCDNModel.Hobby
                            {
                                UserId = newUser.UserId,
                                HobbyName = item.HobbyName
                            };
                            context.Add(newSkill);
                        }
                        if (await context.SaveChangesAsync() <= 0)
                        {
                            dbTransaction.RollbackToSavepoint("BeforeInsertUser");
                            return new Err { HasError = true, ErrMsg = "Register failed" };
                        }
                    }
                    dbTransaction.Commit();
                    return new Err { HasError = false, ErrMsg = "Successful" };
                }
                else
                {
                    dbTransaction.RollbackToSavepoint("BeforeInsertUser");
                    return new Err { HasError = true, ErrMsg = "Register failed" };
                }
                    

            }
            catch (Exception ex)
            {
                dbTransaction.RollbackToSavepoint("BeforeInsertUser");
                return new Err { HasError = true, ErrMsg = ex.Message };
            }

        }
        [HttpPut]
        public async Task<object> UpdateUser(Int32 ID, [FromBody] UpdateParams Params)
        {
            freelanceCDNContext context = new();

            try
            {
                var exist = context.Users.Where(u => u.UserId == ID).FirstOrDefault();
                if (exist == null)
                    return new Err { HasError = true, ErrMsg = "User is not exist" };

                if(Params.Email != null)
                    exist.UserEmail = Params.Email;

                if (Params.PhoneNum != null)
                    exist.UserPhoneNum = Params.PhoneNum;

                var existSkill = context.Skills.Where(s => s.UserId == ID).ToList();
                if (Params.SkillToRemove.Count > 0)
                {
                    foreach (var item in existSkill)
                    {
                        context.Remove(item);
                    }
                }

                if (Params.Skill.Count > 0 && existSkill.Count > 0)
                {
                    foreach (var item in Params.Skill)
                    {
                        if (item.SkillID == null)
                        {
                            var newSkill = new freelanceCDNModel.Skill
                            {
                                UserId = ID,
                                SkillName = item.SkillName
                            };
                            context.Add(newSkill);
                        }
                        else
                        {
                            foreach (var itemdb in existSkill)
                            {
                                if (item.SkillID == itemdb.SkillId)
                                {
                                    itemdb.SkillName = item.SkillName;
                                    context.Update(itemdb);
                                }
                            }
                            
                        }
                    }

                }
                if (Params.Skill.Count > 0 && existSkill.Count <= 0)
                {
                    foreach (var item in Params.Skill)
                    {
                        var newSkill = new freelanceCDNModel.Skill
                        {
                            UserId = ID,
                            SkillName = item.SkillName
                        };
                        context.Add(newSkill);
                    }
                }

                var existHobby = context.Hobbies.Where(h => h.UserId == ID).ToList();
                if (Params.HobbyToRemove.Count > 0)
                {
                    foreach (var item in existHobby)
                    {
                        context.Remove(item);
                    }
                }
                if (Params.Hobby.Count > 0 && existHobby.Count > 0)
                {
                    foreach (var item in Params.Hobby)
                    {
                        if (item.HobbyID == null)
                        {
                            var newHobby = new freelanceCDNModel.Hobby
                            {
                                UserId = ID,
                                HobbyName = item.HobbyName
                            };
                            context.Add(newHobby);
                        }
                        else
                        {
                            foreach (var itemdb in existHobby)
                            {
                                if (item.HobbyID == itemdb.HobbyId)
                                {
                                    itemdb.HobbyName = item.HobbyName;
                                    context.Update(itemdb);
                                }
                            }

                        }
                    }

                }
                if (Params.Hobby.Count > 0 && existHobby.Count <= 0)
                {
                    foreach (var item in Params.Hobby)
                    {
                        var newHobby = new freelanceCDNModel.Hobby
                        {
                            UserId = ID,
                            HobbyName = item.HobbyName
                        };
                        context.Add(newHobby);
                    }
                }


                if (!context.ChangeTracker.HasChanges())
                    return new Err { HasError = true, ErrMsg = "No Update" };

                context.Update(exist);

                if (await context.SaveChangesAsync() > 0)
                    return new Err { HasError = false, ErrMsg = "Successful" };
                else
                    return new Err { HasError = true, ErrMsg = "Update failed" };

            }
            catch (Exception ex)
            {
                return new Err { HasError = true, ErrMsg = ex.Message };
            }

        }
        [HttpDelete]
        public async Task<object> DeleteUser(Int32 ID)
        {
            freelanceCDNContext context = new();

            try
            {
                var exist = context.Users.Where(u => u.UserId == ID).FirstOrDefault();
                if (exist == null)
                    return new Err { HasError = true, ErrMsg = "User is not exist" };

                context.Remove(exist);

                if (await context.SaveChangesAsync() > 0)
                    return new Err { HasError = false, ErrMsg = "Successful" };
                else
                    return new Err { HasError = true, ErrMsg = "Delete failed" };

            }
            catch (Exception ex)
            {
                return new Err { HasError = true, ErrMsg = ex.Message };
            }

        }
    }
}
